using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitPlugin2026.Commands;

/// <summary>
/// Controls when the ribbon button is enabled.
/// Requires an active document to be open.
/// </summary>
public class CommandAvailability : IExternalCommandAvailability
{
    public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories) =>
        applicationData.ActiveUIDocument is not null;
}
