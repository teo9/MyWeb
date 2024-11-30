# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./MyWeb/MyWeb.csproj" --disable-parallel
RUN dotnet publish "./MyWeb/MyWeb.csproj" -c release -o /app --no-restore


# Serve stage 
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS serve
WORKDIR /app
COPY --from=Build /app ./
EXPOSE 8080
RUN ifconfig
ENTRYPOINT [ "dotnet" , "MyWeb.dll" ]

# docker build --rm -t test/myweb:latest .
# docker run --rm -p 8080:80 test/myweb 
# docker run --rm -it -p 8080:8080 -e ASPNETCORE_HTTP_PORTS=8080 aspnetapp