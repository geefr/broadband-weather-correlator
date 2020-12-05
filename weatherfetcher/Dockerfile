FROM mcr.microsoft.com/dotnet/sdk as build-env
MAINTAINER Gareth Francis <gfrancis.dev@gmail.com>

WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "WeatherFetcher"]

