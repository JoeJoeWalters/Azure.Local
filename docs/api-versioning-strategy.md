# API Versioning Strategy (Minimal, Clean Architecture)

## Goal

Enable independent API version evolution (for example `v2`) while keeping `v1` stable and minimizing architecture churn.

## Current Baseline (already in solution)

- Header-based versioning is already configured using `api-version`.
- Default version is `1.0`.
- Timesheet controller is already under `Controllers/V1`.
- Contracts and validators are still shared, which prevents independent version evolution.

## Design Principles

- Keep versioning at the API boundary first.
- Keep `Domain`, `Application`, and `Infrastructure` shared unless business behavior truly diverges.
- Version request/response contracts, validators, mappers, and controllers.
- Reuse shared rule/mapping helpers to reduce duplication.
- Prefer explicit DI registrations over runtime version-switch factories.

## Proposed Structure

```text
src/Azure.Local.ApiService/Timesheets/
  Controllers/
    V1/TimesheetController.cs
    V2/TimesheetController.cs
  Contracts/
    V1/
      Requests/
      Responses/
    V2/
      Requests/
      Responses/
    Shared/                  (optional, small shared primitives only)
  Validators/
    V1/
    V2/
    Shared/                  (rule extensions / common rules)
  Mapping/
    V1/
    V2/
    Shared/                  (common mapping helpers)
```

## Namespace Convention

- `Azure.Local.ApiService.Timesheets.Controllers.V1`
- `Azure.Local.ApiService.Timesheets.Controllers.V2`
- `Azure.Local.ApiService.Timesheets.Contracts.V1.*`
- `Azure.Local.ApiService.Timesheets.Contracts.V2.*`
- `Azure.Local.ApiService.Timesheets.Validators.V1`
- `Azure.Local.ApiService.Timesheets.Validators.V2`
- `Azure.Local.ApiService.Timesheets.Mapping.V1`
- `Azure.Local.ApiService.Timesheets.Mapping.V2`

## Dependency Guidance

- API controllers (V1/V2) depend on:
  - version-specific contracts
  - version-specific validators
  - version-specific mappers
  - shared `ITimesheetApplication`

- Application layer remains unversioned at first:
  - `ITimesheetApplication`
  - `ITimesheetWorkflow`
  - `ITimesheetRepository`

Only introduce `Application.V2` abstractions if business behavior diverges beyond API shape/validation concerns.

## DI Registration Pattern

Register per-version components explicitly:

- `IValidator<V1.Request>` -> `V1.Validator`
- `IValidator<V2.Request>` -> `V2.Validator`
- `V1.ITimesheetContractMapper` -> `V1.TimesheetContractMapper`
- `V2.ITimesheetContractMapper` -> `V2.TimesheetContractMapper`

Each controller uses its own versioned dependencies. Avoid generic object factories keyed by header value in core service registration.

## Migration Plan (Minimal-First)

### Phase 1: Carve out V1 (no behavior change)

- Move current shared contracts into `Contracts/V1`.
- Move validators into `Validators/V1`.
- Keep behavior and routes unchanged.
- Update namespaces/usings and registrations.

### Phase 2: Add V2 surface

- Add `Contracts/V2` with only needed shape differences.
- Add `Validators/V2` using shared rule extensions where possible.
- Add `Mapping/V2` and reuse `Mapping/Shared` helpers.

### Phase 3: Add V2 controller

- Add `Controllers/V2/TimesheetController`.
- Keep same route template and switch behavior by `api-version` header.
- Wire V2 dependencies in DI.

### Phase 4: Harden tests and docs

- Add version negotiation tests (`api-version: 1.0` / `2.0` / default).
- Keep existing V1 component tests.
- Add V2 component tests for V2-only fields/rules.

## Testing Strategy

- Unit tests:
  - validator tests per version (`V1`, `V2`)
  - mapper tests per version
  - shared rule tests once

- Component tests:
  - V1 behavior unchanged
  - V2 behavior and schema differences
  - version negotiation and defaulting behavior

## Tradeoffs and Guardrails

### Tradeoffs

- Some duplication across V1/V2 controllers and contracts is intentional to preserve independent evolution.
- Shared helpers reduce repeated logic without coupling version-specific APIs.

### Guardrails

- Do not leak V2 fields into V1 contracts.
- Do not version the domain model unless business semantics diverge.
- Keep breaking changes isolated to new API versions.
- Keep old versions supported until a formal deprecation/removal policy is in place.

## Summary

The cleanest minimal approach is:

1. Keep header versioning as-is.
2. Version only API boundary artifacts first.
3. Keep application/domain shared.
4. Introduce deeper versioned application abstractions only when business logic actually diverges.

