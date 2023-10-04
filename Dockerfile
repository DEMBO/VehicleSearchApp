FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
EXPOSE 80 
EXPOSE 443

COPY *.sln .
COPY . .
RUN dotnet restore

# testing
FROM build AS testing
WORKDIR /src/VehicleSearch.Api
RUN dotnet build
WORKDIR /src/RdwVehiclesService.Tests
RUN dotnet test
WORKDIR /src/VehicleSearch.Api.IntegrationTests
RUN dotnet test

# publish
FROM testing AS publish
WORKDIR /src/VehicleSearch.Api
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .

COPY localhost.pfx /usr/local/share/ca-certificates/localhost.pfx
RUN update-ca-certificates

ENTRYPOINT ["dotnet", "VehicleSearch.Api.dll"]