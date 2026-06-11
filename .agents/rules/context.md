---
trigger: always_on
---

Estoy desarrollando un sistema de gestión de préstamos para una biblioteca comunitaria llamada "Biblioteca Popular del Barrio".

Stack: C# + .NET 8, Clean Architecture (Domain / Application / Infrastructure / Web), PostgreSQL con EF Core + Npgsql, Redis con StackExchange.Redis, MediatR, FluentValidation, JWT Bearer, React 18 + TypeScript + Vite, TanStack Query, React Router v6, Tailwind + shadcn/ui, Docker Compose.

Estructura de carpetas del backend:
src/
├── SIGEBIC.Domain/
│ ├── Entities/
│ ├── Enums/
│ ├── Interfaces/
│ └── Events/
├── SIGEBIC.Application/
│ ├── Common/Behaviors/
│ ├── Common/Exceptions/
│ └── (módulos con Commands/ y Queries/)
├── SIGEBIC.Infrastructure/
│ ├── Persistence/AppDbContext.cs
│ ├── Persistence/Configurations/
│ ├── Repositories/
│ └── Cache/
└── SIGEBIC.Web/
├── Controllers/
├── Middlewares/
└── Extensions/

Estructura del frontend: frontend/src/ con carpetas api/, components/, hooks/, pages/, context/, types/, lib/.

La Fase 0 (infraestructura base) ya está completa: docker compose levanta correctamente y Swagger responden.
