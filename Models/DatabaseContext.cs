using System.Data.Entity;
using GeolocationAPI.Models.Geoloctaion.Models;
using MySql.Data.Entity;

namespace GeolocationAPI.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DefaultConnection")
        {
            Configuration.ValidateOnSaveEnabled = false;
        }

        public DbSet<Geolocations> Geolocation { get; set; }
    }
}