using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeolocationAPI.Models
{
    namespace Geoloctaion.Models
    {
        public class Language
        {
            public string code { get; set; }
            public string name { get; set; }
            public string native { get; set; }

            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            [ForeignKey("Geolocations")]
            public int languageId { get; set; }

            [Required] public virtual Geolocations Geolocations { get; set; }
        }

        public class Location
        {
            public int geoname_id { get; set; }
            public string capital { get; set; }
            public List<Language> languages { get; set; }
            public string country_flag { get; set; }
            public string country_flag_emoji { get; set; }
            public string country_flag_emoji_unicode { get; set; }
            public string calling_code { get; set; }
            public bool is_eu { get; set; }

            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            [ForeignKey("Geolocations")]
            public int locationId { get; set; }

            [Required] public virtual Geolocations Geolocations { get; set; }
        }

        public class TimeZone
        {
            public string id { get; set; }
            public DateTime current_time { get; set; }
            public int gmt_offset { get; set; }
            public string code { get; set; }
            public bool is_daylight_saving { get; set; }

            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            [ForeignKey("Geolocations")]
            public int timezoneId { get; set; }

            [Required] public virtual Geolocations Geolocations { get; set; }
        }

        public class Currency
        {
            public string code { get; set; }
            public string name { get; set; }
            public string plural { get; set; }
            public string symbol { get; set; }
            public string symbol_native { get; set; }

            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            [ForeignKey("Geolocations")]
            public int currencyId { get; set; }

            [Required] public virtual Geolocations Geolocations { get; set; }
        }

        public class Connection
        {
            public int asn { get; set; }
            public string isp { get; set; }

            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)]
            [ForeignKey("Geolocations")]
            public int connectionId { get; set; }

            [Required] public virtual Geolocations Geolocations { get; set; }
        }

        public class Geolocations
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            public string ip { get; set; }
            public string type { get; set; }
            public string continent_code { get; set; }
            public string continent_name { get; set; }
            public string country_code { get; set; }
            public string country_name { get; set; }
            public string region_code { get; set; }
            public string region_name { get; set; }
            public string city { get; set; }
            public string zip { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public virtual Language Language { get; set; }
            public virtual Location Location { get; set; }
            public virtual TimeZone TimeZone { get; set; }
            public virtual Currency Currency { get; set; }
            public virtual Connection Connection { get; set; }
        }
    }
}