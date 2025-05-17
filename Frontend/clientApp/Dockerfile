FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["clientApp.csproj", "."]
RUN dotnet restore "clientApp.csproj"
COPY . .
RUN dotnet build "clientApp.csproj" -c Release -o /app/build
RUN dotnet publish "clientApp.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"] 