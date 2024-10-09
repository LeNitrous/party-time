using Godot;

namespace Party.Game.Menu;

[Tool]
public sealed partial class Prompt : Control, IModal
{
    [Export]    
    public string Title
    {
        get => this.GetValueAt<string>("%Modal/Header/MarginContainer/HBoxContainer/Title", Label.PropertyName.Text);
        set => this.SetValueAt("%Modal/Header/MarginContainer/HBoxContainer/Title", Label.PropertyName.Text, value);
    }


    [Export]
    public string Text
    {
        get => this.GetValueAt<string>("%Text", Label.PropertyName.Text);
        set => this.SetValueAt("%Text", Label.PropertyName.Text, value);
    }

    [Export]
    public Color Accent
    {
        get => this.GetValueAt<Color>("%Modal/Header/Accent", PropertyName.Modulate);
        set => this.SetValueAt("%Modal/Header/Accent", PropertyName.Modulate, value);
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

    public override void _Ready()
    {
        if (SceneStack.Current is not null)
        {
            SceneStack.Current.Modal = this;
        }

        GetNode<Button>("%Accept").Confirm += accept;
        GetNode<Button>("%Reject").Confirm += reject;
    }

    public override void _ExitTree()
    {
        if (SceneStack.Current is not null && SceneStack.Current.Modal == this)
        {
            SceneStack.Current.Modal = null;
        }
    }

    private void accept()
    {
        EmitSignal(SignalName.Accept);
    }

    private void reject()
    {
        EmitSignal(SignalName.Reject);
    }

    void IModal.Accept()
    {
        accept();
    }

    void IModal.Reject()
    {
        reject();
    }

    [Signal]
    public delegate void AcceptEventHandler();

    [Signal]
    public delegate void RejectEventHandler();
}