using Godot;
using Party.Game.Input;

namespace Party.Game.Menu;

[Tool]
public partial class Modal : Control
{
    [Export]
    public string Title
    {
        get => this.GetValueAt<string>("%Title", Label.PropertyName.Text);
        set => this.SetValueAt("%Title", Label.PropertyName.Text, value);
    }

    [Export(PropertyHint.MultilineText)]
    public string Content
    {
        get => this.GetValueAt<string>("%Content", Label.PropertyName.Text);
        set => this.SetValueAt("%Content", Label.PropertyName.Text, value);
    }

    [Export]
    public Color Accent
    {
        get => this.GetValueAt<Color>("%Header", PropertyName.SelfModulate);
        set => this.SetValueAt("%Header", PropertyName.SelfModulate, value);
    }

    [Export]
    public Texture2D Icon
    {
        get => this.GetValueAt<Texture2D>("%Icon", TextureRect.PropertyName.Texture);
        set => this.SetValueAt("%Icon", TextureRect.PropertyName.Texture, value);
    }

    [Export]
    public bool CanAccept
    {
        get => this.GetValueAt<bool>("%Accept", PropertyName.Visible);
        set => this.SetValueAt("%Accept", PropertyName.Visible, value);
    }

    [Export]
    public bool CanReject
    {
        get => this.GetValueAt<bool>("%Reject", PropertyName.Visible);
        set => this.SetValueAt("%Reject", PropertyName.Visible, value);
    }

    private Control firstSelected;

    private void onVisibilityChanged()
    {
        if (IsInsideTree())
        {
            GetTree().Paused = Visible;
        }

        if (InputManager.Current is not null)
        {
            if (Visible)
            {
                firstSelected = InputManager.Current.FirstSelected;
                InputManager.Current.FirstSelected = GetNode<Control>("%Accept");
            }
            else
            {
                InputManager.Current.FirstSelected = firstSelected;
                firstSelected = null;
            }
        }

        AudioServer.SetBusEffectEnabled(0, 0, Visible);
    }

    private void onAccept()
    {
        EmitSignal(SignalName.Accept);
    }

    private void onReject()
    {
        EmitSignal(SignalName.Reject);
    }

    [Signal]
    public delegate void AcceptEventHandler();

    [Signal]
    public delegate void RejectEventHandler();
}
