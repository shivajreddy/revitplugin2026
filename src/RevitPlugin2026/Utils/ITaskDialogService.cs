using Autodesk.Revit.UI;

namespace RevitPlugin2026.Utils;

/// <summary>
/// Wraps TaskDialog.Show so it can be substituted in unit tests.
/// TaskDialog.Show throws in headless/CI environments.
/// </summary>
public interface ITaskDialogService
{
    void Show(string title, string message);
}

public class RevitTaskDialogService : ITaskDialogService
{
    public void Show(string title, string message) =>
        TaskDialog.Show(title, message);
}
