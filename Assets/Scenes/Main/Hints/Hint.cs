using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    [Flags]
    private enum KeyboardArrowFlags : byte
    {
        None = 0,
        Up = 1 << 1,
        Down = 1 << 2,
        Left = 1 << 3,
        Right = 1 << 4,
        Vertical = Up | Down,
        Horizontal = Left | Right,
        All = Vertical | Horizontal,
    }

    /// <summary>
    /// 
    /// </summary>
    public string Action
    {
        get => actionName;
        set
        {
            actionName = value;
            OnInputSchemeChanged(input);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public string Label
    {
        get => labelText;
        set
        {
            labelText = value;
            OnInputSchemeChanged(input);
        }
    }

    [SerializeField]
    private SpriteAtlas atlas;

    [SerializeField]
    private string actionName;

    [SerializeField]
    private string labelText;

    private bool invalidated;
    private Transform icons;
    private PlayerInput input;
    private TextMeshProUGUI label;

    private void OnInputSchemeChanged(PlayerInput input)
    {
        invalidated = true;
    }

    private void CreateIconObject(string name)
    {
        if (icons == null)
        {
            return;
        }

        if (atlas != null)
        {
            var sprite =  atlas.GetSprite(name);

            if (sprite != null)
            {
                var icon = new GameObject(name);
                var image = icon.AddComponent<Image>();
                image.sprite = sprite;

                var layout = icon.AddComponent<LayoutElement>();
                layout.preferredWidth = 30;
                layout.preferredHeight = 30;

                icon.transform.SetParent(icons);
            }
            else
            {
                Debug.Log($"Missing input hint icon: \"{name}\"");
            }
        }
    }

    private void Start()
    {
        icons = transform.Find("Icons");
        label = transform.Find("Label").GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (input == null)
        {
            input = FindObjectOfType<PlayerInput>();
        }

        input.onControlsChanged += OnInputSchemeChanged;
        OnInputSchemeChanged(input);
    }

    private void OnDisable()
    {
        input.onControlsChanged -= OnInputSchemeChanged;
    }

    private void Update()
    {
        if (invalidated)
        {
            var action = input.actions.FindAction(actionName);

            if (icons != null)
            {
                for (int i = icons.childCount; i > 0; i--)
                {
                    Destroy(icons.GetChild(0).gameObject);
                }
            }

            if (action != null)
            {
                int bindingIndex = action.GetBindingIndex(input.currentControlScheme);
                var binding = action.bindings[bindingIndex];
                var bindingGroup = InputBinding.MaskByGroup(input.currentControlScheme);

                if (binding.isComposite || binding.isPartOfComposite)
                {
                    var keyboardArrowFlags = KeyboardArrowFlags.None;

                    for (; bindingIndex < action.bindings.Count; bindingIndex++)
                    {
                        binding = action.bindings[bindingIndex];

                        if (!bindingGroup.Matches(binding))
                        {
                            break;
                        }

                        var key = binding.ToDisplayString(InputBinding.DisplayStringOptions.DontUseShortDisplayNames);

                        if (keyboard_arrow_flags.TryGetValue(key, out var flag))
                        {
                            keyboardArrowFlags |= flag;
                        }
                        else
                        {
                            CreateIconObject(key);
                        }
                    }

                    if (keyboard_arrow_icons.TryGetValue(keyboardArrowFlags, out string arrowsKey))
                    {
                        CreateIconObject(arrowsKey);
                    }
                }
                else
                {
                    string[] keys = action.GetBindingDisplayString(bindingGroup, InputBinding.DisplayStringOptions.DontUseShortDisplayNames).Split('|', StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < keys.Length; i++)
                    {
                        CreateIconObject(keys[i].Trim());
                    }
                }

                label.text = !string.IsNullOrEmpty(Label) ? Label : action.name;
            }
            else
            {
                label.text = unknown;
            }

            invalidated = false;
        }
    }

    private const string unknown = "<unknown>";

    private static readonly Dictionary<string, KeyboardArrowFlags> keyboard_arrow_flags = new Dictionary<string, KeyboardArrowFlags>
    {
        ["Up Arrow"] = KeyboardArrowFlags.Up,
        ["Down Arrow"] = KeyboardArrowFlags.Down,
        ["Left Arrow"] = KeyboardArrowFlags.Left,
        ["Right Arrow"] = KeyboardArrowFlags.Right,
    };

    private static readonly Dictionary<KeyboardArrowFlags, string> keyboard_arrow_icons = new Dictionary<KeyboardArrowFlags, string>
    {
        [KeyboardArrowFlags.All] = "Arrows",
        [KeyboardArrowFlags.Vertical] = "ArrowsVertical",
        [KeyboardArrowFlags.Horizontal] = "ArrowsHorizontal",
        [KeyboardArrowFlags.Up] = "ArrowsUp",
        [KeyboardArrowFlags.Down] = "ArrowsDown",
        [KeyboardArrowFlags.Left] = "ArrowsLeft",
        [KeyboardArrowFlags.Right] = "ArrowsRight",
    };
}
