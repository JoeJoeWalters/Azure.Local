# Copilot Instructions for Azure.Local

## Project Overview

This is a .NET 10.0 application built using **Clean Architecture** principles with **Aspire** for orchestration. The solution manages timesheets and related business operations with Azure services integration.

## Architecture

The project follows **Clean Architecture** with clear separation of concerns:

### Layer Structure
- **Domain** (`src/domain/Azure.Local.Domain/`)
  - Core business entities and domain logic
  - Pure C# with no external dependencies
  - Contains: Taxonomy, Timesheets

- **Application** (`src/application/Azure.Local.Application/`)
  - Business use cases and orchestration
  - Application services and interfaces
  - Contains: ServiceExtensions, Timesheets logic

- **Infrastructure** (`src/infrastructure/Azure.Local.Infrastructure/`)
  - External concerns (database, messaging, etc.)
  - Repository implementations
  - Contains: Messaging, Repository, ServiceExtensions, Timesheets infrastructure

- **Presentation** (API/Web/Functions)
  - **ApiService**: REST API endpoints (with Swagger/OpenAPI)
  - **Web**: Frontend web application
  - **Functions**: Azure Functions for serverless operations

- **AppHost**: Aspire orchestration and configuration
- **ServiceDefaults**: Shared service configurations

## Technology Stack

- **.NET 10.0** with C# (nullable reference types enabled)
- **Aspire 9.3.1/13.1.0** for cloud-native orchestration
- **Azure Services**:
  - Azure Cosmos DB
  - Azure Service Bus
  - Azure Storage
  - Azure Functions
  - Redis
- **Testing**:
  - xUnit 2.9.3
  - LightBDD 3.11.1 (BDD-style tests)
  - AwesomeAssertions 9.3.0
  - Aspire.Hosting.Testing
  - Coverlet for code coverage
- **Validation**: FluentValidation 12.1.1
- **API Documentation**: Swashbuckle/OpenAPI

## Code Standards

### General Rules
1. **Nullable Reference Types**: Always enabled - handle null cases appropriately
2. **Implicit Usings**: Enabled - common namespaces are auto-imported
3. **Clean Architecture**: Respect layer boundaries - dependencies flow inward only
4. **Project References**: 
   - Domain should have NO external project references
   - Application can reference Domain only
   - Infrastructure can reference Application and Domain
   - Presentation layers can reference all inner layers

### Naming Conventions
- Use **PascalCase** for public members, types, namespaces
- Use **camelCase** for private fields, local variables
- Prefix interfaces with `I`
- Use meaningful, descriptive names

### Code Organization
- Keep domain logic in the Domain layer
- Keep business workflows in the Application layer
- Keep infrastructure concerns (DB, messaging, external APIs) in the Infrastructure layer
- Use dependency injection via `ServiceExtensions.cs` in each layer

## Building and Testing

### Build Commands
```powershell
# Build solution
dotnet build Azure.Local.sln

# Restore packages
dotnet restore
```

### Testing
```powershell
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage" --settings codecoverage.runsettings

# Use utility scripts
.\utils\tests.bat          # Run tests
.\utils\stryker.bat        # Mutation testing
.\utils\clean.bat          # Clean build artifacts
.\utils\cosmosrenew.bat    # Renew Cosmos DB
```

### Test Organization
- **Unit tests**: Located in `tests/Azure.Local.Tests/Unit/`
- **Integration tests**: Use Aspire.Hosting.Testing
- **BDD tests**: Use LightBDD framework
- **Test files**: Stored in `TestFiles/` subdirectories

## Common Patterns

### Dependency Injection
Each layer registers its services in `ServiceExtensions.cs`:
```csharp
public static class ServiceExtensions
{
    public static IServiceCollection AddLayerServices(this IServiceCollection services)
    {
        // Register services here
        return services;
    }
}
```

### Testing with Aspire
Tests can use `Aspire.Hosting.Testing` for integration testing:
```csharp
using Aspire.Hosting.Testing;
using Xunit;

public class IntegrationTests
{
    // Test with distributed application
}
```

### Validation
Use FluentValidation for input validation:
```csharp
public class MyValidator : AbstractValidator<MyModel>
{
    public MyValidator()
    {
        RuleFor(x => x.Property).NotEmpty();
    }
}
```

## Important Notes

### When Working on This Project
1. **Respect Clean Architecture**: Don't reference outer layers from inner layers
2. **Use Aspire patterns**: Leverage Aspire for service orchestration and Azure integration
3. **Write tests**: Follow existing patterns with xUnit and LightBDD
4. **Enable nullable**: All code should handle nullable reference types properly
5. **Use ServiceExtensions**: Register dependencies in the appropriate layer's ServiceExtensions
6. **API changes**: Update Swagger/OpenAPI documentation when modifying endpoints
7. **Test data**: Place test files in appropriate `TestFiles/` directories with proper copy settings

### File Organization
- Don't modify files in `bin/`, `obj/`, or other generated directories
- Utility scripts are in `utils/` folder
- Documentation should go in `docs/` (if needed)
- Follow existing folder structure within each layer

### Azure Resources
- The application uses Azure local emulators for development
- Aspire handles resource provisioning and connection strings
- Use user secrets for sensitive configuration (`UserSecretsId` configured in AppHost)

## Workflow Integration

### GitHub Actions
- `.github/workflows/dotnet.yml`: Main CI/CD pipeline
- `.github/workflows/dependabot-build.yml`: Dependabot build validation
- Dependabot configured for automated dependency updates

### Code Coverage
- Settings in `codecoverage.runsettings`
- Use Coverlet for coverage collection
- Coverage reports excluded from source control

## Quick Reference

### Solution Structure
```
Azure.Local/
├── src/
│   ├── domain/Azure.Local.Domain/           # Entities, value objects
│   ├── application/Azure.Local.Application/ # Use cases, services
│   ├── infrastructure/                      # Data access, external services
│   ├── Azure.Local.ApiService/              # REST API
│   ├── Azure.Local.Web/                     # Web UI
│   ├── Azure.Local.Functions/               # Azure Functions
│   ├── Azure.Local.AppHost/                 # Aspire orchestration
│   └── Azure.Local.ServiceDefaults/         # Shared configs
├── tests/Azure.Local.Tests/                 # All tests
└── utils/                                   # Build/test scripts
```

### Key Files
- `Azure.Local.sln`: Main solution file
- `codecoverage.runsettings`: Code coverage configuration
- `.github/workflows/`: CI/CD pipelines
- `utils/*.bat`: Utility scripts for development

## Best Practices for Copilot Assistance

When helping with this project:
1. **Suggest code that respects layer boundaries**
2. **Use existing patterns** from the codebase
3. **Include proper null checks** (nullable enabled)
4. **Add tests** when implementing new features
5. **Update ServiceExtensions** when adding new services
6. **Follow .NET conventions** and modern C# practices
7. **Consider Aspire patterns** for distributed app concerns
