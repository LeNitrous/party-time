using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Party.Game.Input;

namespace Party.Game.Menu;

public partial class HintController : RichTextLabel
{
    private HintFlags flags;
    private NodePath current;
    private InputSchema schema;
    private List<string> cache;
    private HoverFocusListener hoverFocusListener;

    public override void _Ready()
    {
        if (InputManager.Current is not null)
        {
            InputManager.Current.SchemaChanged += update;
        }

        hoverFocusListener = new HoverFocusListener(hoverFocusChanged);
    }

    public override void _ExitTree()
    {
        if (InputManager.Current is not null)
        {
            InputManager.Current.SchemaChanged -= update;
        }

        hoverFocusListener?.Dispose();
    }

    private void hoverFocusChanged(NodePath path)
    {
        if (path is null || path.IsEmpty)
        {
            update(HintFlags.None);
            return;
        }

        var node = GetNode(path);
        var flag = HintFlags.None;

        if (node is Control control)
        {
            if (control.FocusNeighborTop is not null && !control.FocusNeighborTop.IsEmpty)
            {
                flag |= HintFlags.NavigateUp;
            }

            if (control.FocusNeighborBottom is not null && !control.FocusNeighborBottom.IsEmpty)
            {
                flag |= HintFlags.NavigateDown;
            }

            if (control.FocusNeighborLeft is not null && !control.FocusNeighborLeft.IsEmpty)
            {
                flag |= HintFlags.NavigateLeft;
            }

            if (control.FocusNeighborRight is not null && !control.FocusNeighborRight.IsEmpty)
            {
                flag |= HintFlags.NavigateRight;
            }
        }

        if (node is Button)
        {
            flag |= HintFlags.Select;
        }

        if (node is Slider)
        {
            flag |= HintFlags.Slider;
        }

        if (node is Select)
        {
            flag |= HintFlags.Radio;
        }

        if (SceneStack.Current is not null)
        {
            if (!SceneStack.Current.IsRoot)
            {
                flag |= HintFlags.Back;
            }
        }

        update(flag);
    }

    private void update(InputSchema schema)
    {
        update(schema, flags);
    }

    private void update(HintFlags flags)
    {
        var schema = InputManager.Current is not null ? InputManager.Current.Schema : InputSchema.None;
        update(schema, flags);
    }

    private void update(InputSchema schema, HintFlags flags)
    {
        if (this.schema == schema && flags == this.flags)
        {
            return;
        }

        if (flags is HintFlags.None)
        {
            Text = string.Empty;
            this.flags = HintFlags.None;
            return;
        }

        cache ??= new List<string>();
        cache.Clear();

        if (flags.HasFlag(HintFlags.Back))
        {
            cache.Add(getLocalizedText(schema, HintFlags.Back));
        }

        if (flags.HasFlag(HintFlags.Select))
        {
            cache.Add(getLocalizedText(schema, HintFlags.Select));
        }

        if (flags.HasFlag(HintFlags.Slider))
        {
            cache.Add(getLocalizedText(schema, HintFlags.Slider));
        }

        if (flags.HasFlag(HintFlags.Radio))
        {
            cache.Add(getLocalizedText(schema, HintFlags.Radio));
        }

        if (flags.HasFlag(HintFlags.Navigate))
        {
            cache.Add(getLocalizedText(schema, HintFlags.Navigate));
        }
        else
        {
            if (flags.HasFlag(HintFlags.NavigateVertical))
            {
                cache.Add(getLocalizedText(schema, HintFlags.NavigateVertical));
            }
            else
            {
                if (flags.HasFlag(HintFlags.NavigateUp))
                {
                    cache.Add(getLocalizedText(schema, HintFlags.NavigateUp));
                }

                if (flags.HasFlag(HintFlags.NavigateDown))
                {
                    cache.Add(getLocalizedText(schema, HintFlags.NavigateDown));
                }
            }

            if (flags.HasFlag(HintFlags.NavigateHorizontal))
            {
                cache.Add(getLocalizedText(schema, HintFlags.NavigateHorizontal));
            }
            else
            {
                if (flags.HasFlag(HintFlags.NavigateLeft))
                {
                    cache.Add(getLocalizedText(schema, HintFlags.NavigateLeft));
                }

                if (flags.HasFlag(HintFlags.NavigateRight))
                {
                    cache.Add(getLocalizedText(schema, HintFlags.NavigateRight));
                }
            }
        }

        this.flags = flags;
        this.schema = schema;

        Text = string.Join("   ", cache.Where(s => !string.IsNullOrEmpty(s)));
    }

    private string getLocalizedText(InputSchema schema, HintFlags flag)
    {
        if (!action_mapping.TryGetValue(schema, out var images))
        {
            return string.Empty;
        }

        if (!images.TryGetValue(flag, out string image))
        {
            return string.Empty;
        }

        if (!locale_mapping.TryGetValue(flag, out string locale))
        {
            locale = "<unknown>";
        }

        return string.Format("[img=32]textures/input/{0}/{1}.png[/img] {2}", schema.ToString().ToLowerInvariant(), image, Tr(locale));
    }

    private static readonly Dictionary<HintFlags, string> locale_mapping = new Dictionary<HintFlags, string>
    {
        [HintFlags.Back]               = "UI_ACTION_BACK",
        [HintFlags.Select]             = "UI_ACTION_SELECT",
        [HintFlags.NavigateUp]         = "UI_ACTION_NAVIGATE",
        [HintFlags.NavigateDown]       = "UI_ACTION_NAVIGATE",
        [HintFlags.NavigateLeft]       = "UI_ACTION_NAVIGATE",
        [HintFlags.NavigateRight]      = "UI_ACTION_NAVIGATE",
        [HintFlags.NavigateHorizontal] = "UI_ACTION_NAVIGATE",
        [HintFlags.NavigateVertical]   = "UI_ACTION_NAVIGATE",
        [HintFlags.Navigate]           = "UI_ACTION_NAVIGATE",
        [HintFlags.Slider]             = "UI_ACTION_SLIDER",
        [HintFlags.Radio]              = "UI_ACTION_RADIO",
    };

    private static readonly Dictionary<InputSchema, Dictionary<HintFlags, string>> action_mapping = new Dictionary<InputSchema, Dictionary<HintFlags, string>>
    {
        [InputSchema.Mouse] = new Dictionary<HintFlags, string>
        {
            [HintFlags.Radio]              = "mouse_left",
            [HintFlags.Select]             = "mouse_left",
            [HintFlags.Slider]             = "mouse_scroll",
        },
        [InputSchema.Keyboard] = new Dictionary<HintFlags, string>
        {
            [HintFlags.Back]               = "keyboard_escape",
            [HintFlags.Select]             = "keyboard_enter",
            [HintFlags.NavigateUp]         = "keyboard_arrows_up",
            [HintFlags.NavigateDown]       = "keyboard_arrows_down",
            [HintFlags.NavigateLeft]       = "keyboard_arrows_left",
            [HintFlags.NavigateRight]      = "keyboard_arrows_right",
            [HintFlags.NavigateHorizontal] = "keyboard_arrows_horizontal",
            [HintFlags.NavigateVertical]   = "keyboard_arrows_vertical",
            [HintFlags.Navigate]           = "keyboard_arrows_all",
            [HintFlags.Slider]             = "keyboard_arrows_horizontal",
            [HintFlags.Radio]              = "keyboard_arrows_horizontal",
        },
        [InputSchema.Xbox] = new Dictionary<HintFlags, string>
        {
            [HintFlags.Back]               = "xbox_button_color_b",
            [HintFlags.Select]             = "xbox_button_color_a",
            [HintFlags.NavigateUp]         = "xbox_dpad_up",
            [HintFlags.NavigateDown]       = "xbox_dpad_down",
            [HintFlags.NavigateLeft]       = "xbox_dpad_left",
            [HintFlags.NavigateRight]      = "xbox_dpad_right",
            [HintFlags.NavigateHorizontal] = "xbox_dpad_horizontal",
            [HintFlags.NavigateVertical]   = "xbox_dpad_vertical",
            [HintFlags.Navigate]           = "xbox_dpad_all",
            [HintFlags.Slider]             = "xbox_dpad_horizontal",
            [HintFlags.Radio]              = "xbox_dpad_horizontal",
        },
        [InputSchema.PlayStation] = new Dictionary<HintFlags, string>
        {
            [HintFlags.Back]               = "playstation_button_color_circle",
            [HintFlags.Select]             = "playstation_button_color_cross",
            [HintFlags.NavigateUp]         = "playstation_dpad_up",
            [HintFlags.NavigateDown]       = "playstation_dpad_down",
            [HintFlags.NavigateLeft]       = "playstation_dpad_left",
            [HintFlags.NavigateRight]      = "playstation_dpad_right",
            [HintFlags.NavigateHorizontal] = "playstation_dpad_horizontal",
            [HintFlags.NavigateVertical]   = "playstation_dpad_vertical",
            [HintFlags.Navigate]           = "playstation_dpad_all",
            [HintFlags.Slider]             = "playstation_dpad_horizontal",
            [HintFlags.Radio]              = "playstation_dpad_horizontal",
        },
        [InputSchema.Xbox] = new Dictionary<HintFlags, string>
        {
            [HintFlags.Back]               = "nintendo_button_a",
            [HintFlags.Select]             = "nintendo_button_b",
            [HintFlags.NavigateUp]         = "nintendo_dpad_up",
            [HintFlags.NavigateDown]       = "nintendo_dpad_down",
            [HintFlags.NavigateLeft]       = "nintendo_dpad_left",
            [HintFlags.NavigateRight]      = "nintendo_dpad_right",
            [HintFlags.NavigateHorizontal] = "nintendo_dpad_horizontal",
            [HintFlags.NavigateVertical]   = "nintendo_dpad_vertical",
            [HintFlags.Navigate]           = "nintendo_dpad_all",
            [HintFlags.Slider]             = "nintendo_dpad_horizontal",
            [HintFlags.Radio]              = "nintendo_dpad_horizontal",
        },
        [InputSchema.Touch] = new Dictionary<HintFlags, string>(),
    };

    [Flags]
    private enum HintFlags : ushort
    {
        None = 1 << 0,
        Back = 1 << 1,
        Radio = 1 << 2,
        Select = 1 << 3,
        Slider = 1 << 4,
        NavigateUp = 1 << 5,
        NavigateDown = 1 << 6,
        NavigateLeft = 1 << 7,
        NavigateRight = 1 << 8,
        NavigateHorizontal = NavigateLeft | NavigateRight,
        NavigateVertical = NavigateUp | NavigateDown,
        Navigate = NavigateHorizontal | NavigateVertical,
    }
}
