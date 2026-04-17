using RevitPlugin2026.Utils;

namespace RevitPlugin2026.Tests.Fakes;

/// <summary>
/// Captures TaskDialog calls instead of actually calling Revit's TaskDialog.Show,
/// which throws in headless test environments.
/// </summary>
public class FakeTaskDialogService : ITaskDialogService
{
    public record DialogCall(string Title, string Message);

    private readonly List<DialogCall> _calls = [];

    public IReadOnlyList<DialogCall> Calls => _calls;

    public void Show(string title, string message) =>
        _calls.Add(new DialogCall(title, message));
}
