# BeautyOS — Appointment Service API

Booking engine REST API for a beauty salon.  
Manages **stylists**, **services**, and **appointments** with conflict detection, status transitions, and soft-delete.

---

## Tech stack

- .NET 9, ASP.NET Core Web API
- Entity Framework Core + SQL Server
- FluentValidation (auto-validation)
- AutoMapper
- Swagger / OpenAPI
- Docker + Docker Compose

---

## Quick start (Docker)

The fastest way to run everything — database and API — in one command.

```bash
git clone repo-url && cd Booking

# create your .env from the template
cp .env.example .env

# build and run
docker compose up --build
```

The API starts at **http://localhost:5062/swagger**.  
Migrations apply automatically on startup — no manual steps needed.

### What happens under the hood

1. Docker Compose starts SQL Server and waits until it's healthy.
2. The API container starts, connects to SQL Server, and applies pending EF Core migrations.
3. Swagger UI is available immediately.

---

## Quick start (local, without Docker)

Prerequisites: .NET 9 SDK, SQL Server instance.

```bash
# 1. Set your connection string in appsettings.Development.json
#    (already preconfigured for localhost:1433)

# 2. Run
dotnet run --project Booking.API
```

Migrations apply automatically on startup in Development mode — no manual `dotnet ef` step needed.

Open **https://localhost:7197/swagger** (or http://localhost:5062/swagger).

---

## Environment variables (.env)

| Variable       | Default               | Description                  |
|----------------|-----------------------|------------------------------|
| `SA_PASSWORD`  | `YourStrong!Pass123`  | SQL Server SA password       |
| `DB_NAME`      | `BookingDb`           | Database name                |
| `DB_PORT`      | `1433`                | Host port for SQL Server     |
| `API_PORT`     | `5062`                | Host port for the API        |

---

## API endpoints

Base path: `/api`

### Stylists

| Method | Route                | Description             |
|--------|----------------------|-------------------------|
| GET    | `/api/stylists`      | List all stylists       |
| GET    | `/api/stylists/{id}` | Get stylist by ID       |
| POST   | `/api/stylists`      | Create a stylist        |
| PUT    | `/api/stylists/{id}` | Update a stylist        |
| DELETE | `/api/stylists/{id}` | Delete a stylist        |

### Services

| Method | Route                | Description             |
|--------|----------------------|-------------------------|
| GET    | `/api/services`      | List all services       |
| GET    | `/api/services/{id}` | Get service by ID       |
| POST   | `/api/services`      | Create a service        |
| PUT    | `/api/services/{id}` | Update a service        |
| DELETE | `/api/services/{id}` | Delete a service        |

### Appointments

| Method | Route                            | Description                  |
|--------|----------------------------------|------------------------------|
| GET    | `/api/appointments`              | List (filter by date, stylistId, status; paginated) |
| GET    | `/api/appointments/{id}`         | Get appointment by ID        |
| POST   | `/api/appointments`              | Book a new appointment       |
| PATCH  | `/api/appointments/{id}/status`  | Change appointment status    |
| DELETE | `/api/appointments/{id}`         | Soft-delete an appointment   |

---

## Business rules

### Booking an appointment
- **Conflict detection** — a stylist cannot have two overlapping confirmed appointments (409 Conflict).
- **No past bookings** — `StartsAt` must be in the future (400 Bad Request).
- Stylist and service must exist (404 Not Found).
- New appointments always start as **Pending**.

### Status transitions
Only these transitions are allowed (anything else returns 422):
- Pending → Confirmed
- Pending → Cancelled
- Confirmed → Cancelled

When confirming, the system re-checks for scheduling conflicts.

### Deletion rules
- **Appointments** — always soft-deleted (`DeletedAt` timestamp). Never physically removed.
- **Stylists** — can only be deleted if they have no future non-cancelled appointments (409 otherwise).
- **Services** — can only be deleted if they are not used in any future non-cancelled appointment (409 otherwise).

---

## Project structure

```
Booking.Domain          — Entities, enums (no dependencies)
Booking.Application     — DTOs, interfaces, services, validators, exceptions
Booking.Infrastructure  — EF Core DbContext, repositories, migrations
Booking.API             — Controllers, middleware, Swagger, DI wiring
```

---

## Sample requests

### Create a stylist

```bash
curl -X POST http://localhost:5062/api/stylists \
  -H "Content-Type: application/json" \
  -d '{"name":"Lilit Harutyunyan","specialization":"Colorist","workStart":"09:00","workEnd":"18:00"}'
```

### Create a service

```bash
curl -X POST http://localhost:5062/api/services \
  -H "Content-Type: application/json" \
  -d '{"name":"Balayage","durationMinutes":90,"price":15000}'
```

### Book an appointment

```bash
curl -X POST http://localhost:5062/api/appointments \
  -H "Content-Type: application/json" \
  -d '{"clientName":"Anna Petrosyan","stylistId":1,"serviceId":1,"startsAt":"2026-09-15T10:00:00"}'
```

### Confirm an appointment

```bash
curl -X PATCH http://localhost:5062/api/appointments/1/status \
  -H "Content-Type: application/json" \
  -d '{"status":"Confirmed"}'
```

### List appointments with filters

```bash
curl "http://localhost:5062/api/appointments?date=2026-09-15&stylistId=1&status=Pending&pageNumber=1&pageSize=10"
```
