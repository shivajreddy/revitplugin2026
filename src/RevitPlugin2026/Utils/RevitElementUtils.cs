using Autodesk.Revit.DB;
using RevitPlugin2026.Models;

namespace RevitPlugin2026.Utils;

public interface IRevitElementUtils
{
    IReadOnlyList<ElementData> CollectElementData(Document doc, ICollection<ElementId> ids);
}

public class RevitElementUtils : IRevitElementUtils
{
    public IReadOnlyList<ElementData> CollectElementData(Document doc, ICollection<ElementId> ids)
    {
        var results = new List<ElementData>();
        foreach (var id in ids)
        {
            var el = doc.GetElement(id);
            if (el is null) continue;

            results.Add(new ElementData(
                Id: id.Value,
                Name: el.Name,
                Category: el.Category?.Name ?? "Unknown",
                LevelName: el.LevelId != ElementId.InvalidElementId
                    ? (doc.GetElement(el.LevelId)?.Name ?? "Unknown")
                    : "No Level"));
        }
        return results;
    }
}
