using Autodesk.Revit.UI;
using RevitPlugin2026.Commands;

namespace RevitPlugin2026.UI;

public static class MainRibbonPanel
{
    public static void Register(UIControlledApplication app)
    {
        var panel = app.CreateRibbonPanel("RevitPlugin2026");

        var buttonData = new PushButtonData(
            name: "HelloWorldBtn",
            text: "Hello\nWorld",
            assemblyName: typeof(HelloWorldCommand).Assembly.Location,
            className: typeof(HelloWorldCommand).FullName!)
        {
            ToolTip = "Shows selected element information",
            AvailabilityClassName = typeof(CommandAvailability).FullName,
            LargeImage = LoadBitmapImage("Resources/icon_32.png"),
            Image = LoadBitmapImage("Resources/icon_16.png"),
        };

        panel.AddItem(buttonData);
    }

    private static System.Windows.Media.ImageSource? LoadBitmapImage(string resourcePath)
    {
        try
        {
            var uri = new Uri($"pack://application:,,,/RevitPlugin2026;component/{resourcePath}");
            return new System.Windows.Media.Imaging.BitmapImage(uri);
        }
        catch
        {
            return null;
        }
    }
}
