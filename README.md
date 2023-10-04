# Overview

Build a REST API that lets you search on a license plate to retrieve vehicle information. The API
should contain the following operation:

- Get vehicle information:
    * Input: part of license plate number, brand or type or a combination of these fields.
    * Output: at least the make, model and year of the vehicle

### The information should be coming from the following one API’s:
* Known Vehicles from the RDW: [Open Data RDW: Gekentekende_voertuigen | Open Data | RDW](https://opendata.rdw.nl/Voertuigen/Open-Data-RDW-Gekentekende_voertuigen/m9d7-ebf2)

### Nonfunctional requirements
* The API is protected by an API key.
* The API serves an Open-API spec with a Swagger UI that must be usable to experiment with the API.
* The solution should contain at least one relevant unit test.
* The solution should contain at least one integration test.
* Ensure the application can be monitored in production.

### Bonus
* Persist the data you retrieve via the API’s
* Put the application in a docker image.
* Show it in a CI/CD pipeline how the application is built and deployed.

# Setup

    ApiKey : qlcdWvHiWu0zBWZeP5eUdXIvj4WuVcZK

### Local

    * dotnet restore
    * dotnet build
    * dotnet run --project VehicleSearch.Api/VehicleSearch.Api.csproj

### Docker

    * docker build -f Dockerfile -t vehicle-search-app .
    * docker run -d -p 5147:80 -p 7238:443 -e ASPNETCORE_HTTPS_PORT=7238 -e ASPNETCORE_URLS="https://+;http://+" -e Kestrel__Certificates__Default__Path=/usr/local/share/ca-certificates/localhost.pfx -e Kestrel__Certificates__Default__Password=password vehicle-search-app  
