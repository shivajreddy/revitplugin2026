namespace RevitPlugin2026.Models;

/// <summary>
/// Plain data record with no Revit API types.
/// This is the boundary between the Revit-dependent layer and testable business logic.
/// </summary>
public record ElementData(
    int Id,
    string Name,
    string Category,
    string LevelName);
