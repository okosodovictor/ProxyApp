FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Proxy.Domain/*.csproj ./Proxy.Domain/
COPY Proxy.Infrastructure/*.csproj ./Proxy.Infrastructure/
COPY Proxy.Web/*.csproj ./Proxy.Web/ 
COPY *.sln ./

WORKDIR /app/Proxy.Web
RUN dotnet restore

# Copy everything else and build
COPY . /app
WORKDIR /app/Proxy.Web
RUN dotnet publish -c Release -o out


# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/Proxy.Web/out .
ENTRYPOINT ["dotnet", "Proxy.Web.dll"]