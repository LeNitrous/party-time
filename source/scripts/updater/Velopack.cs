using Godot;
using Velopack;

namespace Party.Game;

public class Velopack 
{
    private const string UPDATE_URL = "https://github.com/LeNitrous/party-time/releases/";

    #nullable enable
    public UpdateInfo? CheckUpdates()
    {
        var updMgr = new UpdateManager(UPDATE_URL);

        if (updMgr == null) return null;
        else return updMgr.CheckForUpdates();
    }

    public static void DownloadUpdate(UpdateInfo info) 
    {
        // ... add bootstrap code here!
    }

    public static void RunEntrypoint() 
    {
        VelopackApp.Build().Run();
    }
}