FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY Booking.sln .
COPY Booking.API/Booking.API.csproj Booking.API/
COPY Booking.Application/Booking.Application.csproj Booking.Application/
COPY Booking.Domain/Booking.Domain.csproj Booking.Domain/
COPY Booking.Infrastructure/Booking.Infrastructure.csproj Booking.Infrastructure/

RUN dotnet restore

COPY . .

RUN dotnet publish Booking.API/Booking.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Booking.API.dll"]
