using FluentAssertions;
using NSubstitute;
using RevitPlugin2026.Models;
using RevitPlugin2026.Utils;
using Xunit;

namespace RevitPlugin2026.Tests.Utils;

public class RevitElementUtilsContractTests
{
    /// <summary>
    /// Verifies that the IRevitElementUtils contract is respected by any substitute.
    /// The concrete RevitElementUtils implementation is tested via integration tests
    /// (requires Revit installed) — see tests/RevitPlugin2026.IntegrationTests.
    /// </summary>
    [Fact]
    public void CollectElementData_WhenSubstituted_ReturnsConfiguredData()
    {
        var expected = new ElementData(42, "Test Wall", "Walls", "Level 1");
        var utils = Substitute.For<IRevitElementUtils>();
        utils.CollectElementData(
                Arg.Any<Autodesk.Revit.DB.Document>(),
                Arg.Any<ICollection<Autodesk.Revit.DB.ElementId>>())
            .Returns([expected]);

        var result = utils.CollectElementData(null!, []);

        result.Should().ContainSingle().Which.Should().Be(expected);
    }

    [Fact]
    public void ElementData_Category_CanBeUnknown()
    {
        var data = new ElementData(1, "Element", "Unknown", "No Level");
        data.Category.Should().Be("Unknown");
        data.LevelName.Should().Be("No Level");
    }
}
