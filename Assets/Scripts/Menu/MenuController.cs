using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private MenuScreen initial;

    [SerializeField]
    private TextMeshProUGUI header;

    [SerializeField]
    private TextMeshProUGUI footer;

    [SerializeField]
    private TextMeshProUGUI hint;

    private MenuScreen current;
    private Stack<MenuScreen> screens;

    private void OnEnable()
    {
        foreach (Transform t in transform)
        {
            if (!t.gameObject.TryGetComponent<MenuScreen>(out var screen))
            {
                continue;
            }

            if (t.gameObject.activeSelf)
            {
                t.gameObject.SetActive(false);
            }
        }

        Next(initial);
    }

    private void HandleSelectionChanged(MenuOption option)
    {
        if (footer != null)
        {
            footer.text = option.Tooltip ?? string.Empty;
        }

        if (hint != null && EventSystem.current.TryGetComponent<InputSystemUIInputModule>(out var component))
        {
            var help = new StringBuilder();
            var mask = InputBinding.MaskByGroups(PlayerInput.all[0].currentControlScheme);

            if (option.Interactive)
            {
                var submit = component.submit.ToInputAction();
                help.Append($"[{submit.GetBindingDisplayString(mask)}] {submit.name}");
                help.Append(' ');
            }

            if (option.Options != null && option.Options.Length > 0)
            {
                help.Append("unnamed");
                help.Append(' ');
            }

            var move = component.move.ToInputAction();
            help.Append($"[{move.GetBindingDisplayString(mask)}] {move.name}");

            hint.text = help.ToString();
        }
    }

    private void HandleScreenChanged(MenuScreen screen)
    {
        if (header != null)
        {
            if (!string.IsNullOrEmpty(screen.Title))
            {
                if (!header.gameObject.activeSelf)
                {
                    header.gameObject.SetActive(true);
                }

                header.text = screen.Title;
            }
            else
            {
                header.gameObject.SetActive(false);
            }
        }
    }

    public void Next(MenuScreen next)
    {
        if (current == next)
        {
            return;
        }

        if (next.gameObject.transform.parent != transform)
        {
            return;
        }

        if (current != null)
        {
            current.OnSelect.RemoveListener(HandleSelectionChanged);
            current.gameObject.SetActive(false);
            screens ??= new Stack<MenuScreen>();
            screens.Push(current);
        }

        current = next;
        current.OnSelect.AddListener(HandleSelectionChanged);
        current.gameObject.SetActive(true);

        HandleScreenChanged(current);
    }

    public void Back()
    {
        if (screens == null)
        {
            return;
        }

        if (!screens.TryPop(out var screen))
        {
            return;
        }

        if (current != null)
        {
            current.gameObject.SetActive(false);
        }

        current = screen;
        current.gameObject.SetActive(true);

        HandleScreenChanged(current);
    }
}

[Serializable]
public class MenuControllerScreenChanged : UnityEvent<MenuScreen>
{
}