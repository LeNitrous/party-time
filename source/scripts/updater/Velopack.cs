using Godot;
using Velopack;

namespace Party.Game;

public class Velopack 
{
    private const string UPDATE_URL = "https://github.com/LeNitrous/party-time/releases/";
    private readonly UpdateManager updMgr; 

    public Velopack()
    {
        updMgr = new UpdateManager(UPDATE_URL);
    }

    public UpdateInfo CheckUpdates()
    {
        return updMgr.CheckForUpdates();
    }

    public static void DownloadUpdate() 
    {
        // ... add bootstrap code here!
    }

    public static void RunEntrypoint() 
    {
        VelopackApp.Build().Run();
    }
}