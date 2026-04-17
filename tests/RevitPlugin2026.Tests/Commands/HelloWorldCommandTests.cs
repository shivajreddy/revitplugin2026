using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using FluentAssertions;
using NSubstitute;
using RevitPlugin2026.Commands;
using RevitPlugin2026.Models;
using RevitPlugin2026.Tests.Fakes;
using Xunit;

namespace RevitPlugin2026.Tests.Commands;

public class HelloWorldCommandTests
{
    private static HelloWorldCommand BuildCommand(
        IEnumerable<ElementData> elements,
        out FakeTaskDialogService dialog)
    {
        dialog = new FakeTaskDialogService();
        return new HelloWorldCommand
        {
            ElementUtils = FakeRevitElementUtils.WithElements(elements.ToArray()),
            TaskDialogService = dialog,
        };
    }

    private static ExternalCommandData BuildCommandData(IEnumerable<ElementId>? ids = null)
    {
        var commandData = Substitute.For<ExternalCommandData>();
        var uiDoc = Substitute.For<UIDocument>(Substitute.For<Document>());
        commandData.Application.ActiveUIDocument.Returns(uiDoc);
        uiDoc.Selection.GetElementIds().Returns(
            (ids ?? Enumerable.Empty<ElementId>()).ToList());
        return commandData;
    }

    [Fact]
    public void Execute_WithElements_ReturnsSucceeded()
    {
        var command = BuildCommand(
            [new ElementData(100, "Wall-1", "Walls", "Level 1")],
            out var dialog);

        string message = string.Empty;
        var result = command.Execute(BuildCommandData(), ref message, new ElementSet());

        result.Should().Be(Result.Succeeded);
    }

    [Fact]
    public void Execute_WithElements_ShowsDialog()
    {
        var command = BuildCommand(
            [
                new ElementData(100, "Wall-1", "Walls", "Level 1"),
                new ElementData(101, "Door-1", "Doors", "Level 1"),
            ],
            out var dialog);

        string message = string.Empty;
        command.Execute(BuildCommandData(), ref message, new ElementSet());

        dialog.Calls.Should().HaveCount(1);
        dialog.Calls[0].Title.Should().Be("RevitPlugin2026");
        dialog.Calls[0].Message.Should().Contain("2 element(s)");
    }

    [Fact]
    public void Execute_WithNoElements_ShowsEmptyDialog()
    {
        var command = BuildCommand([], out var dialog);

        string message = string.Empty;
        command.Execute(BuildCommandData(), ref message, new ElementSet());

        dialog.Calls[0].Message.Should().Contain("0 element(s)");
    }

    [Fact]
    public void Execute_WhenUtilsThrows_ReturnsFailed()
    {
        var dialog = new FakeTaskDialogService();
        var command = new HelloWorldCommand
        {
            ElementUtils = FakeRevitElementUtils.WithException(
                new InvalidOperationException("boom")),
            TaskDialogService = dialog,
        };

        string message = string.Empty;
        var result = command.Execute(BuildCommandData(), ref message, new ElementSet());

        result.Should().Be(Result.Failed);
        message.Should().Be("boom");
        dialog.Calls.Should().BeEmpty();
    }
}
