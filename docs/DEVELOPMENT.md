# Development Guide

## Prerequisites

- **Windows** (required — Revit and WPF)
- **.NET 8 SDK** (`winget install Microsoft.DotNet.SDK.8`)
- **Visual Studio 2022** or **JetBrains Rider**
- **Revit 2026** (for integration tests and manual testing)

## Getting Started

```powershell
git clone <repo-url>
cd RevitPlugin2026
dotnet restore
dotnet build
```

## Local Development Loop

The main project's `Debug` build auto-deploys to your Revit Addins folder via an
MSBuild post-build target. After building in `Debug|x64`:

1. Open Revit 2026
2. The plugin ribbon tab appears automatically
3. Make a code change → rebuild → use **Add-In Manager → "Reload Latest"** in Revit
   (no restart needed for most changes)

## Manual Deployment

Use the PowerShell script for release builds:

```powershell
.\build\Deploy.ps1                      # Release build
.\build\Deploy.ps1 -Configuration Debug # Debug build
```

## Project Structure

```
src/
  RevitPlugin2026/
    Application.cs              # IExternalApplication entry point
    Commands/
      HelloWorldCommand.cs      # IExternalCommand (thin shell, testable via injected deps)
      CommandAvailability.cs    # IExternalCommandAvailability
    Models/
      ElementData.cs            # Plain records — no Revit types (enables unit testing)
    UI/
      MainRibbonPanel.cs        # Ribbon registration
    Utils/
      RevitElementUtils.cs      # Revit API access behind IRevitElementUtils interface
      ITaskDialogService.cs     # TaskDialog wrapper (enables unit testing)

tests/
  RevitPlugin2026.Tests/        # Unit tests — run anywhere, no Revit required
  RevitPlugin2026.IntegrationTests/  # RTF tests — require Revit 2026 installed
```

## Key Architecture Decision: Interface Boundary

The `Models/` layer contains only plain C# records with no Revit API types.
All Revit API access is hidden behind interfaces in `Utils/`. This is what
makes unit testing possible without a running Revit process.

```
Revit types (Document, Element, ElementId)
         │
         ▼
   IRevitElementUtils  ◄─── mocked in unit tests
         │
         ▼
   ElementData, WallData, ...  (plain records)
         │
         ▼
   Business logic  ◄─── fully unit testable
```

## Versioning

Releases are tag-driven: `git tag v1.2.3 && git push --tags` triggers the release workflow.
The tag becomes the `Version`, `AssemblyVersion`, and `FileVersion` via MSBuild properties.
