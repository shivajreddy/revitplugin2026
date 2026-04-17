# Testing Guide

## Testing Strategy Overview

Revit's API types are sealed with no public constructors and throw exceptions outside
a running Revit host process. The project uses a three-tier strategy to handle this.

## Tier 1 — Unit Tests (run anywhere, including CI)

Located in `tests/RevitPlugin2026.Tests/`.

**Key principle:** all business logic sits behind interfaces that accept plain model types
(`ElementData`, etc.), not Revit types. Tests substitute the interfaces with NSubstitute.

```powershell
# Run unit tests only
dotnet test --filter "Category!=Integration"

# With coverage
dotnet test --filter "Category!=Integration" --collect:"XPlat Code Coverage"
```

### TaskDialog isolation

`TaskDialog.Show()` throws in headless environments. All commands accept an
`ITaskDialogService` dependency. Tests use `FakeTaskDialogService` which captures
calls without touching Revit.

### What can't be unit tested

- `RevitElementUtils.CollectElementData` — calls `doc.GetElement()` directly
- `MainRibbonPanel.Register` — calls `UIControlledApplication`
- `Application.OnStartup` / `OnShutdown`

These are covered by Tier 3 integration tests.

## Tier 2 — Smoke Test (manual, Debug build)

After deploying with the Debug build:

1. Open Revit 2026
2. Open any project
3. Click "Hello World" in the RevitPlugin2026 ribbon tab
4. Select elements and verify the dialog shows correct counts and categories
5. Check `%AppData%\RevitPlugin2026\logs\` for log output

## Tier 3 — Integration Tests via RevitTestFramework (RTF)

Located in `tests/RevitPlugin2026.IntegrationTests/`.

RTF launches Revit as a subprocess, loads your test DLL inside the Revit AppDomain,
opens a `.rvt` test model, and runs each test with a real `Document`.

### Prerequisites

```powershell
# Install RTF CLI tool
dotnet tool install -g RevitTestFramework.Console

# Build integration tests
dotnet build tests/RevitPlugin2026.IntegrationTests --configuration Release -p:Platform=x64
```

### Running integration tests

```powershell
RevitTestFramework.exe `
  --dir tests\RevitPlugin2026.IntegrationTests\bin\Release `
  --testFile RevitPlugin2026.IntegrationTests.dll `
  --revit "C:\Program Files\Autodesk\Revit 2026\Revit.exe" `
  --results results.xml `
  --copyAddins true
```

### Adding RTF tests

1. Add the `RevitTestFramework` NuGet package to the integration test project
2. Reference `RTF.Framework` in your test file
3. Decorate tests with `[Trait("Category", "Integration")]` to keep them out of CI
4. Use `[TestModel(@"RTF\TestModel.rvt")]` to specify the Revit model
5. Access the document via `RevitTestExecutive.CommandData`

### Why integration tests are excluded from CI

GitHub-hosted runners do not have Revit installed. RTF tests are run locally by
developers and optionally on self-hosted runners with Revit installed.

To run integration tests on a self-hosted runner, remove the `--filter` flag
from the CI workflow `dotnet test` step and ensure the runner has Revit 2026 installed.

## CI Behavior

| Test Type | CI | Local |
|---|---|---|
| Unit tests (`Category!=Integration`) | Always run | Run with `dotnet test --filter "Category!=Integration"` |
| Integration tests | **Skipped** | Run via RTF CLI |
| Manual smoke test | N/A | After Debug deploy |

## Coverage

Coverage is collected in CI via `coverlet.collector` and uploaded to Codecov.
View coverage locally:

```powershell
dotnet test --filter "Category!=Integration" --collect:"XPlat Code Coverage" --results-directory ./TestResults
# Open TestResults/*/coverage.cobertura.xml in a viewer
reportgenerator -reports:TestResults/*/coverage.cobertura.xml -targetdir:coverage-report
```

(`dotnet tool install -g dotnet-reportgenerator-globaltool`)
