# Flight Status Tracker

A full-stack flight status lookup application built for the SkyRoute platform.

## Stack
- **Backend**: .NET 8 Minimal API (C#)
- **Frontend**: React 19 + TypeScript + Vite + Tailwind CSS
- **Tests**: xUnit + Moq

## Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)

## Run from a clean clone

### 1 — Start the API
```bash
cd FlightStatus.Api
dotnet run
# API listens on http://localhost:5050
```

### 2 — Start the UI (new terminal)
```bash
cd flight-status-ui
npm install
npm run dev
# UI available at http://localhost:5173
```

### 3 — Run the tests
```bash
cd FlightStatus.Tests
dotnet test
```

## Try these sample flights
| Flight | Expected Status | Provider |
|--------|-----------------|----------|
| AA100  | OnTime          | AeroTrack (only) |
| AA200  | Delayed         | AeroTrack |
| AA300  | Cancelled       | AeroTrack |
| AA400  | Diverted        | AeroTrack |
| AA500  | Unknown         | AeroTrack |
| QF100  | OnTime          | QuickFlight wins (newer timestamp) |
| QF200  | Delayed         | AeroTrack wins (newer timestamp) |
| QF300  | Delayed         | QuickFlight (only) |
| XX999  | Unknown         | Neither provider has data |

Use date **2024-06-15** for all stubs.

## Assumptions
- No real flight APIs, auth, or persistence — stubs only
- Stubs are date-agnostic (they return data regardless of the date parameter) to simplify demo
- CORS is open for local development
- Tailwind v4 via `@tailwindcss/vite` plugin (no `tailwind.config.js` needed)
