# Deployment Documentation

## Overview

FocusUp is deployed using Railway and Docker. The application consists of two independent services:

* **Frontend** (Vue.js + Vite)
* **Backend** (.NET 10 API)

Both services are deployed using Docker images hosted on Docker Hub.

---

# Architecture

```text
┌─────────────────────┐
│      Frontend       │
│ focusup.up.railway.app
└──────────┬──────────┘
           │ HTTPS
           ▼
┌─────────────────────┐
│      Backend        │
│ focusup-api.up.railway.app
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ SQLite Database     │
│ /app/Data           │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ Railway Volume      │
└─────────────────────┘
```

---

# Project Structure

```text
project/
├── Backend/
│   └── FocusUp/
│       ├── Dockerfile
│       ├── Dockerfile.railway
│       ├── Program.cs
│       ├── appsettings.example.json
│       └── ...
│
├── Frontend/
│   └── focusUp/
│       ├── Dockerfile
│       ├── package.json
│       ├── vite.config.ts
│       └── src/
│
├── Database/
│   ├── create.sql
│   ├── insert.sql
│   └── productivity_game.sqlite
│
├── docker-compose.yml
└── .env
```

---

# Requirements

## Accounts

The following services are required:

* GitHub
* Docker Hub
* Railway

## Software

Required software:

* Docker Desktop
* Git
* Node.js
* .NET SDK

---

# Railway Setup

Create the following Railway services:

1. `focusup-api`
2. `focusup-frontend`

Both services are deployed independently using Docker images from Docker Hub.

---

# Backend Deployment

## Docker Image

Docker Hub image:

```text
egrf93/focusup-api:latest
```

---

## Railway URL

```text
https://focusup-api.up.railway.app
```

Swagger:

```text
https://focusup-api.up.railway.app/swagger
```

---

## Backend Environment Variables

Configure the following variables in Railway:

```env
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://+:8080

ConnectionStrings__DefaultConnection=Data Source=/app/Data/productivity_game.sqlite
```

---

## Persistent Storage

The application uses SQLite and therefore requires a Railway Volume.

Create a Railway Volume and mount it to:

```text
/app/Data
```

The database file is stored at:

```text
/app/Data/productivity_game.sqlite
```

Without a mounted volume, all data will be lost after redeployment.

---

## Railway Dockerfile

A dedicated Dockerfile is used for Railway deployment:

```text
Backend/FocusUp/Dockerfile.railway
```

---

## Build Backend Image

Run from the project root directory:

```bash
docker build \
-f Backend/FocusUp/Dockerfile.railway \
-t focusup-api .
```

Tag image:

```bash
docker tag focusup-api egrf93/focusup-api:latest
```

Push image:

```bash
docker push egrf93/focusup-api:latest
```

Railway automatically redeploys after a new image is pushed.

---

# Frontend Deployment

## Docker Image

Docker Hub image:

```text
egrf93/focusup-frontend:latest
```

---

## Railway URL

```text
https://focusup.up.railway.app
```

---

## Frontend Environment Variables

Configure:

```env
VITE_API_URL=https://focusup-api.up.railway.app
```

---

## API Configuration

File:

```text
src/api/api.ts
```

Configuration:

```ts
const API_URL = `${import.meta.env.VITE_API_URL}/api`;
```

Example API request:

```text
https://focusup-api.up.railway.app/api/Auth/login
```

---

## Vite Configuration

File:

```text
vite.config.ts
```

Configuration:

```ts
server: {
  host: "0.0.0.0",
  port: 5173,
  allowedHosts: true
}
```

This allows Railway domains to access the Vite server.

---

## Build Frontend Image

Navigate to the frontend directory:

```bash
cd Frontend/focusUp
```

Build image:

```bash
docker build -t focusup-frontend .
```

Tag image:

```bash
docker tag focusup-frontend egrf93/focusup-frontend:latest
```

Push image:

```bash
docker push egrf93/focusup-frontend:latest
```

Railway automatically redeploys after a new image is pushed.

---

# CORS Configuration

The backend must allow requests from both the local frontend and the deployed frontend.

File:

```text
Program.cs
```

Configuration:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "https://focusup.up.railway.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
```

---

# Local Development

The project can still be started locally using Docker Compose.

Run from the project root:

```bash
docker compose up --build
```

---

## Local URLs

Frontend:

```text
http://localhost:5173
```

Backend:

```text
http://localhost:5165
```

Swagger:

```text
http://localhost:5165/swagger
```

---

# Security

## appsettings.json

Do not commit:

```text
appsettings.json
```

Instead commit:

```text
appsettings.example.json
```

Add to `.gitignore`:

```gitignore
**/appsettings.json
```

If the file is already tracked:

```bash
git rm --cached Project/Backend/FocusUp/appsettings.json
```

---

## Node Modules

The following files should be committed:

```text
package.json
package-lock.json
```

The following folder must never be committed:

```text
node_modules/
```

Add to `.gitignore`:

```gitignore
node_modules/
```

---

# Deployment Workflow

## Backend

```bash
cd project

docker build \
-f Backend/FocusUp/Dockerfile.railway \
-t focusup-api .

docker tag focusup-api egrf93/focusup-api:latest

docker push egrf93/focusup-api:latest
```

---

## Frontend

```bash
cd Frontend/focusUp

docker build -t focusup-frontend .

docker tag focusup-frontend egrf93/focusup-frontend:latest

docker push egrf93/focusup-frontend:latest
```

---

# Production URLs

Frontend:

```text
https://focusup.up.railway.app
```

Backend:

```text
https://focusup-api.up.railway.app
```

Swagger:

```text
https://focusup-api.up.railway.app/swagger
```

API Base URL:

```text
https://focusup-api.up.railway.app/api
```
