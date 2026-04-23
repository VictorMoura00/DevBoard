# DevBoard

DevBoard is a personal and open source project manager built as a modular monolith with ASP.NET Core and PostgreSQL.

The main goal of this repository is to keep the project easy to run locally, easy to review, and easy to evolve toward the MVP without unnecessary distributed-system complexity.

## Stack

- .NET 10
- ASP.NET Core Minimal APIs
- PostgreSQL
- Docker Compose

## Project structure

```text
src/
  DevBoard.Api
  DevBoard.Infrastructure
  DevBoard.Modules.Notifications
  DevBoard.Modules.Projects
  DevBoard.Modules.Tasks
  DevBoard.SharedKernel

docs/
tests/
```

## Run with Docker

1. Copy `.env.example` to `.env` if you want to customize ports or credentials.
2. Start the stack:

```bash
docker compose up --build
```

Available endpoints after startup:

- API: `http://localhost:8080`
- Health check: `http://localhost:8080/health`
- Scalar docs: `http://localhost:8080/docs` (Development)

## Run locally without Docker

Start only PostgreSQL with Docker:

```bash
docker compose up -d db
```

Then run the API:

```bash
dotnet run --project src/DevBoard.Api
```

## Build

```bash
dotnet build DevBoard.slnx
```

## Test

```bash
dotnet test DevBoard.slnx
```

## Standards

Project standards and architectural rules live in:

- [docs/development-standards.md](docs/development-standards.md)
- [docs/mvp-plan.md](docs/mvp-plan.md)
- [docs/github-setup.md](docs/github-setup.md)

## Ownership

Repository owner: [VictorMoura00](https://github.com/VictorMoura00)
