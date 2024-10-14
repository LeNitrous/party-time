using Godot;
using Party.Game.Input;

namespace Party.Game.Menu;

public sealed partial class Title : Node
{
    private bool primed;
    private RichTextLabel label;

    public override void _Ready()
    {
        if (InputManager.Current is not null)
        {
            InputManager.Current.SchemaChanged += onSchemaChanged;
            onSchemaChanged(InputManager.Current.Schema);
        }
    }

    public override void _ExitTree()
    {
        if (InputManager.Current is not null)
        {
            InputManager.Current.SchemaChanged -= onSchemaChanged;
        }
    }

    private void onSchemaChanged(InputSchema schema)
    {
        (label ??= GetNode<RichTextLabel>("Label")).Text = $"[center]{getActionForSchema(schema)}[/center]";
    }

    private void onGUIInput(InputEvent e)
    {
        if (e is InputEventMouseButton m)
        {
            if (m.Pressed)
            {
                primed = true;
            }

            if (!m.Pressed && primed)
            {
                primed = false;
                switchToMainMenu();
            }
        }

        if (e.IsActionReleased("ui_select"))
        {
            switchToMainMenu();
        }
    }

    private void switchToMainMenu()
    {
        SceneStack.Current?.Push("res://scenes/menu_main.tscn", false);
    }

    private string getActionForSchema(InputSchema schema)
    {
        return schema switch
        {
            InputSchema.Touch => Tr("UI_TITLE_ACTION_MOBILE"),
            _ => string.Format(Tr("UI_TITLE_ACTION"), getButtonForSchema(schema)),
        };
    }

    private string getButtonForSchema(InputSchema schema)
    {
        string button = schema switch
        {
            InputSchema.Mouse => "textures/input/mouse/mouse_left.png",
            InputSchema.Keyboard => "textures/input/keyboard/keyboard_space.png",
            InputSchema.Xbox => "textures/input/xbox/xbox_button_color_a.png",
            InputSchema.Gamepad => "textures/input/xbox/xbox_button_color_a.png",
            InputSchema.Nintendo => "textures/input/nintendo/nintendo_button_b.png",
            InputSchema.PlayStation => "textures/input/playstation/playstation_button_color_cross.png",
            _ => string.Empty,
        };

        return $"[img=32]{button}[/img]";
    }
}