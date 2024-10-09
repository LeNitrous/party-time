using Godot;
using Party.Game.Experience;
using Party.Game.Experience.Managers;

namespace Party.Game.Menu;

public sealed partial class Results : Control, IModal
{
    private bool canPauseGameplay;
    private Label title;
    private Control color;
    private Button buttonReturn;
    private Button buttonResume;
    private Button buttonContinue;

    public override void _Ready()
    {
        buttonReturn = GetNode<Button>("%Return");
        buttonReturn.Confirm += returnToMainMenu;
       
        buttonResume = GetNode<Button>("%Resume");
        buttonResume.Confirm += resumeToGameplay;
        
        buttonContinue = GetNode<Button>("%Continue");
        buttonContinue.Confirm += returnToMainMenu;

        title = GetNode<Label>("%Modal/Header/MarginContainer/HBoxContainer/Title");
        title.Text = Tr("UI_MODAL_TITLE_RESULTS");

        color = GetNode<Control>("%Modal/Header/Accent");
        color.Modulate = Color.Color8(0, 170, 255);
    }

    public override void _Input(InputEvent e)
    {
        if (e.IsActionReleased("ui_cancel"))
        {
            if (Visible)
            {
                resumeToGameplay();
            }
        }
    }

    public override void _Notification(int what)
    {
        if (what == NotificationVisibilityChanged)
        {
            if (IsInsideTree())
            {
                if (canPauseGameplay)
                {
                    GetTree().Paused = Visible;
                }

                if (GetTree().Paused)
                {
                    title.Text = Tr("UI_MODAL_TITLE_PAUSED");
                    buttonReturn.Visible = true;
                    buttonResume.Visible = true;
                    buttonContinue.Visible = false;
                }
                else
                {
                    title.Text = Tr("UI_MODAL_TITLE_RESULTS");
                    buttonReturn.Visible = false;
                    buttonResume.Visible = false;
                    buttonContinue.Visible = true;
                }
            }

            if (Visible)
            {
                if (!canPauseGameplay)
                {
                    GetNode("Popup").Call(Tween.MethodName.Play);
                }

                if (GameStateManager.Current is not null)
                {
                    var state = GameStateManager.Current;
                    GetNode<Label>("%ScoreCurrent").Text = state.Score.ToString();
                    GetNode<Label>("%ScoreMaximum").Text = $"/{state.Round}";
                    GetNode<Label>("%ComboCurrent").Text = state.Combo.ToString();
                    GetNode<Label>("%ComboMaximum").Text = $" ({state.ComboMaximum} MAX)";
                    GetNode<Label>("%Duration").Text = state.Duration.ToString(@"%m\:ss");
                }
            }
        }
    }

    private void onPhaseChanged(Phase phase)
    {
        if (phase is Phase.Prologue)
        {
            if (SceneStack.Current is not null)
            {
                SceneStack.Current.Modal = this;
            }
        }

        if (phase is Phase.Epilogue)
        {
            if (SceneStack.Current is not null && SceneStack.Current.Modal == this)
            {
                SceneStack.Current.Modal = null;
            }

            CallDeferred(CanvasItem.MethodName.Show);
            GetNode<Control>("Blur").CallDeferred(CanvasItem.MethodName.Hide);
            GetNode<Control>("Background").CallDeferred(CanvasItem.MethodName.Hide);
        }

        canPauseGameplay = phase is not Phase.Epilogue;
    }

    private void resumeToGameplay()
    {
        CallDeferred(CanvasItem.MethodName.Hide);
        GetTree().Paused = false;
    }

    private void returnToMainMenu()
    {
        GetTree().Paused = false;

        if (SceneStack.Current is not null)
        {
            if (SceneStack.Current.Exit("res://scenes/menu_main.tscn"))
            {
                return;
            }
            else
            {
                SceneStack.Current.Exit();
            }
        }
    }

    void IModal.Accept()
    {
        returnToMainMenu();
    }

    void IModal.Reject()
    {
        resumeToGameplay();
    }
}