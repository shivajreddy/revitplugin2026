using System.Globalization;
using System.IO;
using Autodesk.Revit.UI;
using RevitPlugin2026.UI;
using Serilog;

namespace RevitPlugin2026;

/// <summary>
/// Entry point for the Revit external application.
/// Revit calls OnStartup once at load time, OnShutdown when Revit closes.
/// </summary>
public class Application : IExternalApplication
{
    internal static ILogger Logger { get; private set; } = Serilog.Core.Logger.None;

    public Result OnStartup(UIControlledApplication application)
    {
        Logger = new LoggerConfiguration()
            .WriteTo.File(
                path: Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "RevitPlugin2026", "logs", "plugin-.log"),
                formatProvider: CultureInfo.InvariantCulture,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7)
            .WriteTo.Debug(formatProvider: CultureInfo.InvariantCulture)
            .CreateLogger();

        Logger.Information("RevitPlugin2026 starting (Revit {Version})",
            application.ControlledApplication.VersionNumber);

        try
        {
            MainRibbonPanel.Register(application);
            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Failed during OnStartup");
            return Result.Failed;
        }
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        Logger.Information("RevitPlugin2026 shutting down");
        Log.CloseAndFlush();
        return Result.Succeeded;
    }
}
