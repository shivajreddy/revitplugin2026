using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitPlugin2026.Utils;

namespace RevitPlugin2026.Commands;

/// <summary>
/// Minimal IExternalCommand that reads selected elements and shows a task dialog.
/// Business logic is delegated to IRevitElementUtils so it can be unit-tested
/// without a running Revit process.
/// </summary>
[Transaction(TransactionMode.ReadOnly)]
[Regeneration(RegenerationOption.Manual)]
public class HelloWorldCommand : IExternalCommand
{
    // Injected for testability; defaults to the real implementations.
    internal IRevitElementUtils ElementUtils { get; set; } = new RevitElementUtils();
    internal ITaskDialogService TaskDialogService { get; set; } = new RevitTaskDialogService();

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        try
        {
            var doc = commandData.Application.ActiveUIDocument.Document;
            var ids = commandData.Application.ActiveUIDocument.Selection.GetElementIds();
            var data = ElementUtils.CollectElementData(doc, ids);

            TaskDialogService.Show(
                "RevitPlugin2026",
                $"Selected {data.Count} element(s).\n" +
                string.Join("\n", data.Select(e => $"  [{e.Id}] {e.Name} ({e.Category})")));

            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            Application.Logger.Error(ex, "HelloWorldCommand failed");
            message = ex.Message;
            return Result.Failed;
        }
    }
}
