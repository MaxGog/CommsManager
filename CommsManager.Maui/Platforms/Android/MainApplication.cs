using Android.App;
using Android.Runtime;

namespace CommsManager.Maui.Platforms.Android;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override void OnCreate()
    {
        base.OnCreate();

        InitDatabase();
    }

    private void InitDatabase()
    {
        var appData = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
        var imagesDir = Path.Combine(appData, "images");
        Directory.CreateDirectory(imagesDir);
    }
}