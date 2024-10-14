using System.Threading.Tasks;
using Godot;
using Party.Game.Camera;

namespace Party.Game.Menu;

public sealed partial class PermissionCheck : Node
{
    public override void _Ready()
    {
        if (CameraService.Current is not null)
        {
            if (CameraService.Current.PermissionGranted())
            {
                onAccept();
            }
            else
            {
                var prompt = GetNode<Prompt>("Prompt");
                prompt.GetNode<Button>("%Accept").Confirm += () => _ = onRequestPermission();
                prompt.Show();
                GD.Print(nameof(PermissionCheck), " :: permission requested");
            }
        }
        else
        {
            onAccept();
        }
    }

    private async Task onRequestPermission()
    {
        bool granted = await CameraService.Current.RequestPermission();

        if (granted)
        {
            CallDeferred(MethodName.onAccept);
        }
        else
        {
            CallDeferred(MethodName.onReject);
        }
    }

    private void onAccept()
    {
        GD.Print(nameof(PermissionCheck), " :: permission granted");
        SceneStack.Current.Push("res://scenes/splash.tscn", false);
    }

    private void onReject()
    {
        GD.Print(nameof(PermissionCheck), " :: permission rejected");
        SceneStack.Current.Exit();
    }
}