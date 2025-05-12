FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /src
COPY ./invoiceService.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c release -o /app

#Working dir

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet","invoiceService.dll"]
