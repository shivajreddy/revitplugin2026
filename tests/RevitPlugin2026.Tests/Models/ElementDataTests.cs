using FluentAssertions;
using RevitPlugin2026.Models;
using Xunit;

namespace RevitPlugin2026.Tests.Models;

public class ElementDataTests
{
    [Fact]
    public void ElementData_SameValues_AreEqual()
    {
        var a = new ElementData(1, "Wall", "Walls", "Level 1");
        var b = new ElementData(1, "Wall", "Walls", "Level 1");
        a.Should().Be(b);
    }

    [Fact]
    public void ElementData_DifferentIds_AreNotEqual()
    {
        var a = new ElementData(1, "Wall", "Walls", "Level 1");
        var b = new ElementData(2, "Wall", "Walls", "Level 1");
        a.Should().NotBe(b);
    }

    [Fact]
    public void ElementData_WithExpression_ProducesNewRecord()
    {
        var original = new ElementData(1, "Wall", "Walls", "Level 1");
        var updated = original with { Name = "Wall-Renamed" };

        updated.Id.Should().Be(1);
        updated.Name.Should().Be("Wall-Renamed");
        updated.Should().NotBe(original);
    }
}
