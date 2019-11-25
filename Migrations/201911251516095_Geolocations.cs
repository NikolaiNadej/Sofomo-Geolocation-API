using System.Data.Entity.Migrations;

namespace GeolocationAPI.Migrations
{
    public partial class Geolocations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                    "dbo.Geolocations",
                    c => new
                    {
                        Id = c.Int(false, true),
                        ip = c.String(unicode: false),
                        type = c.String(unicode: false),
                        continent_code = c.String(unicode: false),
                        continent_name = c.String(unicode: false),
                        country_code = c.String(unicode: false),
                        country_name = c.String(unicode: false),
                        region_code = c.String(unicode: false),
                        region_name = c.String(unicode: false),
                        city = c.String(unicode: false),
                        zip = c.String(unicode: false),
                        latitude = c.String(unicode: false),
                        longitude = c.String(unicode: false)
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                    "dbo.Connections",
                    c => new
                    {
                        connectionId = c.Int(false),
                        asn = c.Int(false),
                        isp = c.String(unicode: false)
                    })
                .PrimaryKey(t => t.connectionId)
                .ForeignKey("dbo.Geolocations", t => t.connectionId)
                .Index(t => t.connectionId);

            CreateTable(
                    "dbo.Currencies",
                    c => new
                    {
                        currencyId = c.Int(false),
                        code = c.String(unicode: false),
                        name = c.String(unicode: false),
                        plural = c.String(unicode: false),
                        symbol = c.String(unicode: false),
                        symbol_native = c.String(unicode: false)
                    })
                .PrimaryKey(t => t.currencyId)
                .ForeignKey("dbo.Geolocations", t => t.currencyId)
                .Index(t => t.currencyId);

            CreateTable(
                    "dbo.Languages",
                    c => new
                    {
                        languageId = c.Int(false),
                        code = c.String(unicode: false),
                        name = c.String(unicode: false),
                        native = c.String(unicode: false),
                        Location_locationId = c.Int()
                    })
                .PrimaryKey(t => t.languageId)
                .ForeignKey("dbo.Geolocations", t => t.languageId)
                .ForeignKey("dbo.Locations", t => t.Location_locationId)
                .Index(t => t.languageId)
                .Index(t => t.Location_locationId);

            CreateTable(
                    "dbo.Locations",
                    c => new
                    {
                        locationId = c.Int(false),
                        geoname_id = c.Int(false),
                        capital = c.String(unicode: false),
                        country_flag = c.String(unicode: false),
                        country_flag_emoji = c.String(unicode: false),
                        country_flag_emoji_unicode = c.String(unicode: false),
                        calling_code = c.String(unicode: false),
                        is_eu = c.Boolean(false)
                    })
                .PrimaryKey(t => t.locationId)
                .ForeignKey("dbo.Geolocations", t => t.locationId)
                .Index(t => t.locationId);

            CreateTable(
                    "dbo.TimeZones",
                    c => new
                    {
                        timezoneId = c.Int(false),
                        id = c.String(unicode: false),
                        current_time = c.DateTime(false, 0),
                        gmt_offset = c.Int(false),
                        code = c.String(unicode: false),
                        is_daylight_saving = c.Boolean(false)
                    })
                .PrimaryKey(t => t.timezoneId)
                .ForeignKey("dbo.Geolocations", t => t.timezoneId)
                .Index(t => t.timezoneId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.TimeZones", "timezoneId", "dbo.Geolocations");
            DropForeignKey("dbo.Languages", "Location_locationId", "dbo.Locations");
            DropForeignKey("dbo.Locations", "locationId", "dbo.Geolocations");
            DropForeignKey("dbo.Languages", "languageId", "dbo.Geolocations");
            DropForeignKey("dbo.Currencies", "currencyId", "dbo.Geolocations");
            DropForeignKey("dbo.Connections", "connectionId", "dbo.Geolocations");
            DropIndex("dbo.TimeZones", new[] {"timezoneId"});
            DropIndex("dbo.Locations", new[] {"locationId"});
            DropIndex("dbo.Languages", new[] {"Location_locationId"});
            DropIndex("dbo.Languages", new[] {"languageId"});
            DropIndex("dbo.Currencies", new[] {"currencyId"});
            DropIndex("dbo.Connections", new[] {"connectionId"});
            DropTable("dbo.TimeZones");
            DropTable("dbo.Locations");
            DropTable("dbo.Languages");
            DropTable("dbo.Currencies");
            DropTable("dbo.Connections");
            DropTable("dbo.Geolocations");
        }
    }
}