using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using GeolocationAPI.Models;
using GeolocationAPI.Models.Geoloctaion.Models;
using Newtonsoft.Json;

namespace GeolocationAPI.Controllers
{
    [RoutePrefix("api")]
    public class WebApiController : ApiController
    {
        private readonly string ApiKey = "27ea7696ac71229a47d2f930883c513f";

        private void CheckExternalApi()
        {
            string info;
            try
            {
                info = new WebClient().DownloadString("http://api.ipstack.com/188.121.0.14?access_key=" + ApiKey);
            }
            catch (Exception e)
            {
                var response =
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                        "Error downloading response. Check connectivity with ipstack.com");
                throw new HttpResponseException(response);
            }

            if (info.Contains("You have not supplied a valid API Access Key"))
            {
                var response =
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                        "Check if you provided valid API key");
                throw new HttpResponseException(response);
            }
        }

        [HttpPost]
        [Route("AddGeolocationData")]
        public IHttpActionResult AddGeolocationData(HttpRequestMessage request)
        {
            CheckExternalApi();
            try
            {
                var json = new WebClient().DownloadString("http://api.ipstack.com/" +
                                                          request.Content.ReadAsStringAsync().Result + "?access_key=" +
                                                          ApiKey);

                var content = JsonConvert.DeserializeObject<Geolocations>(json);

                using (var db = new DatabaseContext())
                {
                    var geolocation = db.Set<Geolocations>();
                    geolocation.Add(content);

                    db.SaveChanges();
                }

                return Ok();
            }
            catch
            {
                var response =
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
                throw new HttpResponseException(response);
            }
        }

        [HttpDelete]
        [Route("DeleteGeolocationData")]
        public IHttpActionResult DeleteGeolocationData(string ip)
        {
            HttpResponseMessage response = null;
            try
            {
                var ipRegex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
                if (!ipRegex.IsMatch(ip))
                    return BadRequest("Not valid id");

                using (var db = new DatabaseContext())
                {
                    var record = db.Geolocation
                        .Where(s => s.ip == ip)
                        .FirstOrDefault();

                    if (record == null)
                    {
                        response =
                            Request.CreateErrorResponse(HttpStatusCode.InternalServerError,
                                "IP Address does not exist in database.");
                        throw new HttpResponseException(response);
                    }

                    //delete all children records
                    if (record.Connection != null)
                        db.Entry(record.Connection).State = EntityState.Deleted;

                    if (record.Currency != null)
                        db.Entry(record.Currency).State = EntityState.Deleted;

                    if (record.Language != null)
                        db.Entry(record.Language).State = EntityState.Deleted;

                    if (record.Location != null)
                        db.Entry(record.Location).State = EntityState.Deleted;

                    if (record.TimeZone != null)
                        db.Entry(record.TimeZone).State = EntityState.Deleted;

                    db.Entry(record).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
            catch
            {
                if (response == null) //check if no previous error occured
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
                    throw new HttpResponseException(response);
                }
            }

            return Ok();
        }

        [HttpGet]
        [Route("GetGeolocationData")]
        public IHttpActionResult GetGeolocationData(string ip)
        {
            HttpResponseMessage response = null;
            try
            {
                var ipRegex = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");
                if (!ipRegex.IsMatch(ip))
                    return BadRequest("Not valid id");

                using (var db = new DatabaseContext())
                {
                    var record = db.Geolocation
                        .Where(s => s.ip == ip)
                        .FirstOrDefault();

                    if (record == null)
                    {
                        response =
                            Request.CreateErrorResponse(HttpStatusCode.NotFound,
                                "IP Address does not exist in database.");
                        throw new HttpResponseException(response);
                    }

                    //prevent self loop
                    if (record.Connection?.Geolocations != null)
                        record.Connection.Geolocations = null;
                    if (record.Currency?.Geolocations != null)
                        record.Currency.Geolocations = null;
                    if (record.Location?.Geolocations != null)
                        record.Location.Geolocations = null;
                    if (record.Language?.Geolocations != null)
                        record.Language.Geolocations = null;
                    if (record.TimeZone?.Geolocations != null)
                        record.TimeZone.Geolocations = null;


                    return Ok(JsonConvert.SerializeObject(record));
                }
            }
            catch
            {
                if (response == null) //check if no previous error occured
                {
                    response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
                    throw new HttpResponseException(response);
                }

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                    return InternalServerError();
            }

            return NotFound();
        }
    }
}