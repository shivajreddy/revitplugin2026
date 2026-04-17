using Autodesk.Revit.DB;
using FluentAssertions;
using RevitPlugin2026.Utils;
using Xunit;

namespace RevitPlugin2026.IntegrationTests.RTF;

/// <summary>
/// Integration tests that run INSIDE Revit via RevitTestFramework (RTF).
///
/// Prerequisites:
///   - Revit 2026 installed at C:\Program Files\Autodesk\Revit 2026\Revit.exe
///   - RTF CLI: dotnet tool install -g RevitTestFramework.Console
///
/// Run locally:
///   RevitTestFramework.exe \
///     --dir tests\RevitPlugin2026.IntegrationTests\bin\Release \
///     --testFile RevitPlugin2026.IntegrationTests.dll \
///     --revit "C:\Program Files\Autodesk\Revit 2026\Revit.exe" \
///     --results results.xml \
///     --copyAddins true
///
/// These tests are tagged [Trait("Category","Integration")] so CI skips them
/// via --filter "Category!=Integration".
/// </summary>
[Trait("Category", "Integration")]
public class HelloWorldCommandIntegrationTests
{
    /// <summary>
    /// Verifies that RevitElementUtils correctly reads walls from a real Revit document.
    /// This test requires RTF to inject the Document from TestModel.rvt.
    /// </summary>
    [Fact]
    public void CollectElementData_WallsInTestModel_ReturnsWallData()
    {
        // RTF opens TestModel.rvt and provides the document via the Revit process.
        // Access it through the static RTF executive (set up by the RTF test runner).
        // Replace the line below with the RTF-specific document access pattern
        // once RevitTestFramework NuGet is added.
        //
        // var doc = RevitTestExecutive.CommandData.Application.ActiveUIDocument.Document;
        //
        // For now this serves as the integration test scaffold — add RTF NuGet reference
        // and uncomment when running locally.
        throw new SkipException("Scaffold only — requires RTF runner and Revit 2026 installed.");
    }

    [Fact]
    public void CollectElementData_WithRealDocument_CategoryIsPopulated()
    {
        throw new SkipException("Scaffold only — requires RTF runner and Revit 2026 installed.");
    }
}

/// <summary>Placeholder to allow the file to compile before RTF NuGet is added.</summary>
public class SkipException(string reason) : Exception(reason);
