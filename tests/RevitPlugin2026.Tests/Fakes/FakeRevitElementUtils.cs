using Autodesk.Revit.DB;
using NSubstitute;
using RevitPlugin2026.Models;
using RevitPlugin2026.Utils;

namespace RevitPlugin2026.Tests.Fakes;

/// <summary>
/// Factory for test doubles of IRevitElementUtils.
/// Because Document is sealed with no public constructor, we mock the interface
/// rather than trying to fake the Revit types directly.
/// </summary>
public static class FakeRevitElementUtils
{
    public static IRevitElementUtils WithElements(params ElementData[] data)
    {
        var fake = Substitute.For<IRevitElementUtils>();
        fake.CollectElementData(
                Arg.Any<Document>(),
                Arg.Any<ICollection<ElementId>>())
            .Returns(data.ToList());
        return fake;
    }

    public static IRevitElementUtils WithException(Exception ex)
    {
        var fake = Substitute.For<IRevitElementUtils>();
        fake.CollectElementData(
                Arg.Any<Document>(),
                Arg.Any<ICollection<ElementId>>())
            .Throws(ex);
        return fake;
    }
}
