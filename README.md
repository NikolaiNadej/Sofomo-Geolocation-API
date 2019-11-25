# Sofomo-Geolocation-API
### ASP.NET Example geolocation api
___

#### Pre-Configuration:
* Install Web Server + MySQL (XAMPP or any other web server solution stack package can come in handy)
* Turn on web server and MySQL (MySQL must be running on port `3306`)

Database login is `root` without password. (`userid=root;password=`)
You can change default login by modifying `connectionString` in `Web.config`.
___
#### Sofomo-Geolocation-API - ASP.NET WEB API (.NET Framework `4.5.2`) + Entity Framework Backend
* Run Visual Studio as Administrator
* Make sure you have .NET Framework `4.5.2` installed.
* Open NuGet Package Manager > Package Manager Console
* Restore missing packages
* Run Solution

MySQL Database will be created automatically.
Backend is running on port `44364` by default. 
___
#### Test Api calls:
POST `https://localhost:44364/api/AddGeolocationData` | Body = `162.158.63.5`
GET `https://localhost:44364/api/GetGeolocationData?ip=162.158.63.5`
DELETE `https://localhost:44364/api/DeleteGeolocationData?ip=162.158.63.5`