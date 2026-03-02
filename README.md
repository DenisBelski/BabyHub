# BabyHub API

REST API for managing newborn patient records with FHIR-compliant date search.

## Tech Stack

- .NET 6, ASP.NET Core Web API
- Entity Framework Core + MS SQL Server
- Docker + Docker Compose

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## Running the Application

1. Clone the repository:
```bash
git clone <repository-url>
cd BabyHub
```

2. Start the containers:
```bash
docker-compose up --build
```

3. Open Swagger UI:
```
http://localhost:5000/swagger
```

## Seeding Test Data (100 patients)

Make sure the API is running, then execute:
```bash
docker-compose --profile seeder run --rm seeder
```

## Stopping the Application

```bash
docker-compose down
```

To also remove the database volume:
```bash
docker-compose down -v
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/patients` | Create a patient |
| GET | `/api/patients/{id}` | Get patient by ID |
| PUT | `/api/patients/{id}` | Update a patient |
| DELETE | `/api/patients/{id}` | Delete a patient |
| GET | `/api/patients?birthDate=...` | Search by birth date |

## FHIR Date Search

Supports all 9 FHIR date search operators. Full specification: https://www.hl7.org/fhir/search.html#date

| Query | Description |
|-------|-------------|
| `?birthDate=eq2013-01-14` | Born on Jan 14, 2013 |
| `?birthDate=ne2013-01-14` | Not born on Jan 14, 2013 |
| `?birthDate=lt2013-01-14` | Born before Jan 14, 2013 |
| `?birthDate=gt2013-01-14` | Born after Jan 14, 2013 |
| `?birthDate=le2013-01-14` | Born on or before Jan 14, 2013 |
| `?birthDate=ge2013-01-14` | Born on or after Jan 14, 2013 |
| `?birthDate=sa2013-01-14` | Starts after Jan 14, 2013 |
| `?birthDate=eb2013-01-14` | Ends before Jan 14, 2013 |
| `?birthDate=ap2013-01-14` | Approximately Jan 14, 2013 |

## Postman Collection

Import the collection file `BabyHub.postman_collection.json` located in the repository root.

**Import steps:**
1. Open Postman
2. `File` → `Import` → select `BabyHub.postman_collection.json`

**Base URL** is pre-configured as a collection variable:
- Default value: `http://localhost:5000`
- To change: click the collection → **Variables** tab → update `baseUrl`

**Running CRUD requests:**

Run in order — `Create Patient` automatically saves the patient ID to the `patientId` variable, which is used by all subsequent requests.

1. `Create Patient` — creates a new patient
2. `Get Patient by ID` — retrieves the created patient
3. `Update Patient` — updates patient data
4. `Delete Patient` — deletes the patient

**Date search requests** demonstrate all 9 FHIR operators: `eq`, `ne`, `lt`, `gt`, `le`, `ge`, `sa`, `eb`, `ap`.
