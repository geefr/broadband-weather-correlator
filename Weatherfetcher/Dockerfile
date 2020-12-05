FROM mcr.microsoft.com/dotnet/sdk as build-env
MAINTAINER Gareth Francis <gfrancis.dev@gmail.com>

WORKDIR /app

COPY ./WeatherFetcher/*.csproj ./
RUN dotnet restore

COPY ./WeatherFetcher/* ./
RUN ls -a
RUN dotnet publish -c Release -o out --no-restore

FROM mcr.microsoft.com/dotnet/runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["/app/WeatherFetcher"]

