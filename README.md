# SIGEBIC — Sistema de Gestión Bibliotecaria

**SIGEBIC** es una aplicación web moderna para la gestión integral de bibliotecas. Permite administrar el catálogo de libros, controlar préstamos y devoluciones, gestionar multas por retrasos, y mantener un historial completo de todas las transacciones.

---

## 📋 Tabla de Contenidos

- [Stack Tecnológico](#stack-tecnológico)
- [Arquitectura General](#arquitectura-general)
- [Backend: SIGEBIC (.NET)](#backend-sigebic-net)
  - [Estructura de Capas](#estructura-de-capas)
  - [Patrones Implementados](#patrones-implementados)
  - [Flujo de una Petición](#flujo-de-una-petición)
- [Frontend: React SPA](#frontend-react-spa)
  - [Estructura del Proyecto](#estructura-del-proyecto)
  - [Patrones y Flujo](#patrones-y-flujo)
- [Flujo Integral (Ejemplo)](#flujo-integral-ejemplo)
- [Diagrama de Arquitectura](#diagrama-de-arquitectura)
- [Cómo Empezar](#cómo-empezar)
  - [Requisitos](#requisitos)
  - [Ejecución en Desarrollo](#ejecución-en-desarrollo)
  - [Ejecución con Docker](#ejecución-con-docker)
- [Módulos del Sistema](#módulos-del-sistema)
- [Estructura del Repositorio](#estructura-del-repositorio)

---

## 🛠️ Stack Tecnológico

### Backend

| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| .NET | 8.0 | Framework principal |
| ASP.NET Core | 8.0 | API REST |
| Entity Framework Core | 8.0 | ORM y acceso a datos |
| MediatR | — | Bus de mensajería CQRS in-process |
| FluentValidation | — | Validación declarativa |
| SQL Server | — | Base de datos relacional |
| JWT Bearer | — | Autenticación y autorización |

### Frontend

| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| React | 19 | Librería de UI |
| TypeScript | 6 | Tipado estático |
| Vite | 8 | Bundler y dev server |
| Tailwind CSS | 4 | Estilos utilitarios |
| shadcn/ui | — | Componentes de UI (Radix primitives) |
| TanStack React Query | 5 | Manejo de estado asíncrono y cache |
| Axios | — | Cliente HTTP |
| React Router | 7 | Enrutamiento SPA |

### Infraestructura

| Tecnología | Propósito |
|------------|-----------|
| Docker | Contenerización |
| Docker Compose | Orquestación multi-servicio |
| Nginx | Reverse proxy y static serving |

---

## 🏗️ Arquitectura General

SIGEBIC es una aplicación **monolítica modular** que sigue una **Arquitectura Limpia (Clean Architecture)** combinando **Domain-Driven Design (DDD)** con **CQRS (Command Query Responsibility Segregation)** usando **MediatR** como bus de mensajería in-process.

```
┌─────────────────────────────────────────────────────────────┐
│               Navegador (React SPA)                         │
│  Pages → Hooks (React Query) → API (Axios) → Proxy /api/*  │
└──────────────────────────┬──────────────────────────────────┘
                           │ HTTP
                           ▼
┌─────────────────────────────────────────────────────────────┐
│  SIGEBIC.Web (ASP.NET Core 8) — Controladores REST          │
│  ↓ MediatR                                                   │
│  SIGEBIC.Application — Commands/Queries + Handlers           │
│  ↓ Interfaces (puertos)                                      │
│  SIGEBIC.Domain — Entidades, Eventos, Enums, Specs           │
│  ↓ Implementaciones                                          │
│  SIGEBIC.Infrastructure — EF Core → SQL Server              │
└─────────────────────────────────────────────────────────────┘
```

---

## 🔧 Backend: SIGEBIC (.NET)

### Estructura de Capas

```
SIGEBIC/
├── SIGEBIC.slnx
└── src/
    ├── SIGEBIC.Domain/          ← Núcleo del negocio
    ├── SIGEBIC.Application/     ← Casos de uso (CQRS)
    ├── SIGEBIC.Infrastructure/  ← Acceso a datos y servicios
    └── SIGEBIC.Web/             ← API REST y configuración
```

#### SIGEBIC.Domain (Núcleo del Negocio)

Contiene las entidades, enumeraciones, eventos de dominio e interfaces (puertos) que definen el corazón del sistema.

- **Entidades**: `Prestamo`, `Libro`, `Usuario`, `Multa`, `HistorialPrestamo` — con lógica de negocio encapsulada
- **Enums**: `EstadoPrestamo` (Activo, Devuelto, Vencido, Renovado), `EstadoMulta` (Pendiente, Pagada)
- **Eventos de Dominio**: `PrestamoRegistradoEvent`, `PrestamoDevueltoEvent`, `PrestamoVencidoEvent`
- **Interfaces (Puertos)**: `IPrestamoRepository`, `ILibroRepository`, `IUnitOfWork`, `IMultaRepository`
- **Specifications**: `IPrestamoSpecification` para consultas filtradas reutilizables

#### SIGEBIC.Application (Casos de Uso)

Implementa la lógica de orquestación mediante Commands y Queries siguiendo el patrón CQRS con MediatR.

- **Commands**: `RegistrarPrestamoCommand`, `RegistrarDevolucionCommand`, `RenovarPrestamoCommand`, `GenerarMultaCommand`, `RegistrarPagoMultaCommand`
- **Queries**: `GetPrestamosQuery`, `GetPrestamoByIdQuery`, `GetMisPrestamosQuery`, `GetPrestamosVencidosQuery`, `GetMultasByUsuarioQuery`, etc.
- **Handlers**: Cada Command/Query tiene su propio Handler
- **Validators**: FluentValidation para cada Command
- **DTOs**: `PrestamoDto`, `MultaDto`, `HistorialPrestamoDto`
- **Event Handlers**: Reaccionan a eventos de dominio (ej: `GenerarMultaCommandHandler` al vencerse un préstamo)

#### SIGEBIC.Infrastructure (Infraestructura)

Implementa los puertos definidos en Domain. Contiene el contexto de EF Core, repositorios concretos y configuraciones.

- **AppDbContext**: DbContext de EF Core con configuración Fluent API
- **Configurations**: `PrestamoConfiguration`, `MultaConfiguration`, `HistorialPrestamoConfiguration`
- **Repositories**: Implementaciones concretas (`PrestamoRepository`, `MultaRepository`, `HistorialPrestamoRepository`)
- **UnitOfWork**: Coordina transacciones entre múltiples repositorios
- **Specification Evaluator**: Evalúa specifications contra IQueryable de EF Core

#### SIGEBIC.Web (API REST)

Capa de presentación que expone la funcionalidad vía endpoints REST.

- **Controllers**: `PrestamosController`, `MultasController`, `HistorialController`, `LibrosController`, `UsuariosController`, `AuthController`
- **Program.cs**: Configuración de DI, MediatR, JWT Auth, CORS, Health Checks
- **Extensions**: `InfrastructureExtensions` para registro de servicios

### Patrones Implementados

| Patrón | Descripción |
|--------|-------------|
| **CQRS** | Separación total entre operaciones de escritura (Commands) y lectura (Queries) |
| **MediatR** | Bus in-process que enruta automáticamente cada Command/Query a su Handler |
| **Repository** | Abstracción de persistencia con interfaces en Domain e implementaciones en Infrastructure |
| **Unit of Work** | Transacciones atómicas que coordinan múltiples repositorios |
| **Specification** | Criterios de consulta reutilizables y componibles |
| **Domain Events** | Eventos de dominio que permiten reacciones desacopladas a cambios en el negocio |
| **FluentValidation** | Reglas de validación declarativas y separadas de los objetos de comando |
| **DTO** | Objetos de transferencia que evitan exponer las entidades del dominio directamente |

### Flujo de una Petición

```
Cliente HTTP (Frontend)
       │
       ▼
  ┌───────────────┐
  │  Controller   │  ← Recibe request, construye Command/Query
  └───────┬───────┘
          │ MediatR.Send()
          ▼
  ┌───────────────────┐
  │  Command/Query     │  ← Objeto inmutable con datos de entrada
  └───────┬───────────┘
          │ MediatR
          ▼
  ┌───────────────────────┐
  │  Handler (Application)│  ← Orquestación del caso de uso
  │  - Valida (FluentVal.)│
  │  - Invoca repositorios│
  │  - Publica eventos    │
  └───────┬───────────────┘
          ▼
  ┌────────────────────┐
  │  Repository (Infra) │  ← EF Core
  └───────┬────────────┘
          ▼
    ┌───────────┐
    │ SQL Server│
    └───────────┘
```

---

## 🎨 Frontend: React SPA

### Estructura del Proyecto

```
frontend/
├── src/
│   ├── api/              → Capa HTTP con Axios (1 archivo por módulo)
│   │   ├── auth.api.ts
│   │   ├── libros.api.ts
│   │   ├── multas.api.ts
│   │   ├── prestamos.api.ts
│   │   └── usuarios.api.ts
│   ├── components/
│   │   ├── layout/       → sidebar, header, footer, dashboard-layout
│   │   ├── shared/       → ProtectedRoute
│   │   └── ui/           → shadcn/ui (button, card, input, dialog, etc.)
│   ├── context/          → AuthContext (JWT)
│   ├── hooks/            → Custom hooks con React Query
│   │   ├── useAuth.ts
│   │   ├── useLibros.ts
│   │   ├── useMultas.ts
│   │   ├── usePrestamos.ts
│   │   └── useUsuarios.ts
│   ├── lib/              → Instancia Axios, helpers
│   ├── pages/            → Vistas organizadas por feature
│   │   ├── libros/       → CatalogoPage
│   │   ├── multas/       → MisMultasPage, MultasPendientesPage, RegistrarPagoModal
│   │   ├── prestamos/    → PrestamosActivosPage, MisPrestamosPage, etc.
│   │   └── DashboardPage.tsx
│   ├── types/            → Interfaces TypeScript (DTOs)
│   ├── App.tsx           → Router principal
│   └── main.tsx          → Entry point
```

### Patrones y Flujo

| Patrón | Implementación |
|--------|---------------|
| **Custom Hooks + React Query** | Cada entidad tiene un hook que encapsula queries y mutations con cache automático |
| **Axios Interceptors** | Inyección automática de JWT, manejo global de errores 401 |
| **Protected Routes** | Componente que verifica autenticación vía AuthContext |
| **Modular por Feature** | Cada módulo de negocio tiene su propia carpeta en pages/ |
| **Composición shadcn/ui** | Componentes atómicos reutilizables con variantes |

```
Usuario → Página → Hook (React Query) → API (Axios) → Backend
                                                         │
                    UI ← Cache update ← Response ←──────┘
```

---

## 🔄 Flujo Integral (Ejemplo)

### Caso: Usuario solicita un préstamo de libro

```
1. FRONTEND: Usuario llena formulario en RegistrarPrestamoModal
2. FRONTEND: usePrestamos.useRegistrarPrestamo()  →  mutation
3. FRONTEND: prestamos.api.registrar(data)  →  POST /api/prestamos
4. BACKEND: PrestamosController.Registrar(RegistrarPrestamoCommand)
5. BACKEND: MediatR enruta → RegistrarPrestamoCommandHandler
6. BACKEND: Valida (FluentValidation), crea entidad Prestamo (estado "Activo")
7. BACKEND: PrestamoRepository.Add(prestamo) → EF Core → SQL Server
8. BACKEND: Publica PrestamoRegistradoEvent
9. BACKEND: EventHandler reacciona (ej: notificaciones)
10. BACKEND: Retorna PrestamoDto
11. FRONTEND: React Query invalida cache → UI se refresca
```

---

## 📊 Diagrama de Arquitectura

```
┌──────────────────────────────────────────────────────────────────┐
│                        CLIENTE (Browser)                         │
│                    React SPA (frontend/)                         │
│  React Router → Pages → Custom Hooks (React Query) → Axios API  │
└──────────────────────────────┬───────────────────────────────────┘
                               │ HTTP /api/*
                               ▼
┌──────────────────────────────────────────────────────────────────┐
│                 NGINX (Production) / Vite Proxy (Dev)            │
└──────────────────────────────┬───────────────────────────────────┘
                               ▼
┌──────────────────────────────────────────────────────────────────┐
│              SIGEBIC.Web (ASP.NET Core 8)                        │
│  Controllers → MediatR → Handlers → Repositories → EF Core      │
│  JWT Auth │ CORS │ Health Checks                                │
├──────────────────────────────────────────────────────────────────┤
│  SIGEBIC.Application (CQRS + MediatR + FluentValidation)        │
├──────────────────────────────────────────────────────────────────┤
│  SIGEBIC.Domain (Entidades, Eventos, Interfaces)                │
├──────────────────────────────────────────────────────────────────┤
│  SIGEBIC.Infrastructure (EF Core, SQL Server, Redis)            │
└──────────────────────────────┬───────────────────────────────────┘
                               ▼
                    ┌───────────────────┐
                    │   SQL Server      │
                    │   (Base Datos)    │
                    └───────────────────┘
```

---

## 🚀 Cómo Empezar

### Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (opcional)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (local o Docker)

### Ejecución en Desarrollo

#### 1. Backend

```bash
# Navegar al proyecto Web
cd SIGEBIC/src/SIGEBIC.Web

# Restaurar dependencias
dotnet restore

# Aplicar migraciones (asegúrate de tener SQL Server corriendo)
dotnet ef database update

# Ejecutar
dotnet run
```

El backend estará disponible en `http://localhost:8080`.

#### 2. Frontend

```bash
# Navegar al frontend
cd frontend

# Instalar dependencias
npm install

# Ejecutar en modo desarrollo
npm run dev
```

El frontend estará disponible en `http://localhost:5173`. Las peticiones a `/api/*` se redirigirán automáticamente al backend gracias al proxy configurado en `vite.config.ts`.

### Ejecución con Docker

```bash
# Construir y levantar todos los servicios
docker-compose up --build

# Para ejecutar en segundo plano
docker-compose up -d
```

Esto levantará:
- **Backend**: `http://localhost:8080`
- **Frontend**: `http://localhost`
- **SQL Server**: Puerto `1433`

---

## 📦 Módulos del Sistema

### 📚 Libros
- Gestión del catálogo (CRUD)
- Búsqueda y filtros por título, autor, género
- Control de disponibilidad y cantidad de ejemplares

### 📖 Préstamos
- Registro de préstamos con fecha de devolución
- Devolución de libros
- Renovación de préstamos
- Detección automática de préstamos vencidos

### 💰 Multas
- Generación automática de multas por retraso
- Consulta de multas pendientes por usuario
- Registro de pago de multas

### 📜 Historial
- Historial completo de préstamos por libro
- Historial de actividad por usuario

### 👥 Usuarios
- Gestión de usuarios (CRUD)
- Roles y permisos (Bibliotecario, Estudiante, etc.)
- Autenticación JWT

---

## 📁 Estructura del Repositorio

```
SIGEBIC/
├── README.md                    ← Este archivo
├── docker-compose.yml           ← Orquestación Docker
├── nginx/                       ← Configuración Nginx
│   └── nginx.conf
├── SIGEBIC/                     ← Backend .NET
│   ├── SIGEBIC.slnx
│   └── src/
│       ├── SIGEBIC.Domain/
│       ├── SIGEBIC.Application/
│       ├── SIGEBIC.Infrastructure/
│       └── SIGEBIC.Web/
├── frontend/                    ← Frontend React
│   ├── src/
│   ├── DockerFile
│   ├── nginx.conf
│   └── package.json
├── Docs/                        ← Documentación adicional
│   ├── Casos de uso/
│   ├── Diagrama de Clases UML/
│   └── Diagrama ERD/
└── Pruebas_Serenity/            ← Pruebas E2E con Serenity BDD
```

---

## Licencia

Este proyecto está bajo la licencia MIT. Consulta el archivo `LICENSE` para más detalles.