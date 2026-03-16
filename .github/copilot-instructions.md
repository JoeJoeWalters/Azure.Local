# Copilot Instructions for Azure.Local

## Package Management

NuGet package versions are controlled centrally via `Directory.Packages.props` at the solution root (Central Package Management). **Do not add `Version=` attributes to `<PackageReference>` elements in `.csproj` files.** Add new packages to `Directory.Packages.props` as `<PackageVersion Include="..." Version="..." />` first, then reference them versionlessly in the csproj.

## Build & Test

```powershell
dotnet build Azure.Local.sln
dotnet test ./tests/Azure.Local.Tests/Azure.Local.Tests.csproj

# Run a single test class
dotnet test ./tests/Azure.Local.Tests/Azure.Local.Tests.csproj --filter "FullyQualifiedName~ApplicationUnitTests"

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage" --settings codecoverage.runsettings

# Coverage report (local HTML)
.\utils\tests.bat

# Mutation testing
.\utils\stryker.bat
```

## Architecture

Clean Architecture (.NET 10) orchestrated with **Aspire**. Dependency direction: `Domain ← Application ← Infrastructure ← ApiService/Web/Functions`. `AppHost` wires everything together using local emulators (Cosmos DB, Service Bus, Redis, Storage).

```
src/
  domain/Azure.Local.Domain/          # Entities only – no external deps
  application/Azure.Local.Application/ # Business logic, ITimesheetApplication
  infrastructure/Azure.Local.Infrastructure/ # Cosmos DB, Service Bus, Repository
  Azure.Local.ApiService/             # MVC controllers + FluentValidation
  Azure.Local.Web/                    # Frontend (Blazor/Razor)
  Azure.Local.Functions/              # Azure Functions (Service Bus triggers)
  Azure.Local.AppHost/                # Aspire orchestration
  Azure.Local.ServiceDefaults/        # Shared telemetry/health config
tests/Azure.Local.Tests/
  Unit/Timesheets/                    # xUnit [Fact] tests against application layer
  Component/Timesheets/               # LightBDD [Scenario] tests via WebApplicationFactory
```

## Key Conventions

### Repository pattern
All data access goes through `ITimesheetRepository` (defined in **Application**), implemented in Infrastructure by `TimesheetCosmosRepository`. The domain-to-repository-item mapping (`CastHelper`) lives in `Infrastructure/Timesheets/CastHelper.cs`. `IRepository<T>` / `CosmosRepository<T>` remain as a generic Cosmos DB primitive used internally by `TimesheetCosmosRepository`.

### Dependency direction
`Domain ← Application ← Infrastructure ← ApiService/Web/Functions`. Application defines interfaces (`ITimesheetRepository`, `ITimesheetWorkflow`, `ITimesheetFileProcessor`) that Infrastructure implements. Application has **no reference** to Infrastructure.

### API routes
All timesheet endpoints are rooted at `/person/{personId}/timesheet/...`. Controllers use primary constructor injection and store injected services in `readonly` fields.

### Service registration
Each layer exposes a `ServiceExtensions.cs` with extension methods (e.g., `AddApplication()`, `AddInfrastructure()`). The ApiService's `Program.cs` composes these. Use **C# 14 `extension` blocks** (not traditional `static class` extension methods) when adding new helpers to `ServiceExtensions.cs` — see `tests/Azure.Local.Tests/Component/Setup/ServiceExtensions.cs` for the pattern.

### Component tests
- Use `WebApplicationFactory<Program>` with fakes replacing real infrastructure.
- Replace `IRepository<T>` with `FakeRepository<T>` and `IServiceBusClient` with `FakeServiceBusClient` via `services.RemoveAll<>() + AddSingleton<>()` in `ConfigureTestServices`.
- All component test classes must be `[Collection("NoParallelization")]`.
- Scenarios use LightBDD's `Runner.RunScenarioAsync` with `given/when/then/and` steps defined as methods (snake_case method names map to readable scenario steps).
- `ComponentTestBase<T>` pre-configures `HttpClient` with `x-ms-client-request-id` and `Accept: application/json` headers.

### Assertions
Use **AwesomeAssertions** (not FluentAssertions). It is globally imported via `Global.cs` (`global using AwesomeAssertions`).

### Nullable & usings
- Nullable reference types are **enabled** everywhere — always handle null cases.
- Common namespaces are pre-imported via `Global.cs` files (e.g., `global using AwesomeAssertions; global using System.Diagnostics.CodeAnalysis`).
- Mark test helpers and infrastructure with `[ExcludeFromCodeCoverage]`.

### `Program` partial class
`Program` in `Azure.Local.ApiService` is declared `partial` to allow `WebApplicationFactory<Program>` in tests. Keep this when modifying `Program.cs`.

### Timesheet state machine
State transitions (Draft → Submitted → Approved/Rejected; Recalled → Draft) are enforced in `TimesheetWorkflow`. Domain entities expose `CanSubmit()`, `CanApprove()`, `CanReject()`, `CanRecall()` guards. Approve locks all components (`IsLocked = true`); Reopen unlocks them.
