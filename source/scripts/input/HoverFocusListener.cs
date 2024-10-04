using System;
using Godot;

namespace Party.Game.Input;

public sealed class HoverFocusListener : IDisposable
{
    private bool isDisposed;
    private NodePath focused;
    private NodePath hovered;
    private readonly Action<NodePath> hoverFocusChanged;

    public HoverFocusListener(Action<NodePath> hoverFocusChanged)
    {
        if (InputManager.Current is not null)
        {
            InputManager.Current.FocusChanged += focusChanged;
            InputManager.Current.HoverChanged += hoverChanged;
        }

        this.hoverFocusChanged = hoverFocusChanged;
    }

    private void focusChanged(NodePath path)
    {
        focused = path is null || path.IsEmpty ? null : path;
        update();
    }

    private void hoverChanged(NodePath path)
    {
        hovered = path is null || path.IsEmpty ? null : path;
        update();
    }

    private void update()
    {
        hoverFocusChanged?.Invoke(focused ?? hovered);
    }

    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }

        if (InputManager.Current is not null)
        {
            InputManager.Current.FocusChanged -= focusChanged;
            InputManager.Current.HoverChanged -= hoverChanged;
        }

        isDisposed = true;
    }
}
