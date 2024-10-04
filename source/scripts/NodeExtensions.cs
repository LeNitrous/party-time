using System;
using System.Collections.Generic;
using Godot;

namespace Party.Game;

public static class NodeExtensions
{
    public static void SetAndRaise<T>(this Node node, ref T field, T value)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;

        node.NotifyPropertyListChanged();
    }

    public static TValue GetValueAt<[MustBeVariant] TValue>(this Node node, NodePath path, StringName property)
    {
        if (path.IsEmpty)
        {
            throw new ArgumentException("Path cannot be empty", nameof(path));
        }

        if (property.IsEmpty)
        {
            throw new ArgumentException("Property name cannot be empty", nameof(property));
        }

        var target = node.GetNodeOrNull(path);

        if (target is null)
        {
            return default;
        }

        return target.Get(property).As<TValue>();
    }

    public static void SetValueAt<[MustBeVariant] TValue>(this Node node, NodePath path, StringName property, TValue value)
    {
        void getNodeAndSetValue(NodePath path, StringName property, TValue value)
        {
            var target = node.GetNodeOrNull(path);

            if (target is null)
            {
                return;
            }

            node.GetNode(path).Set(property, Variant.From(value));
        }

        void setter()
        {
            getNodeAndSetValue(path, property, value);
            node.Ready -= setter;
        }

        if (path.IsEmpty)
        {
            throw new ArgumentException("Path cannot be empty", nameof(path));
        }

        if (property.IsEmpty)
        {
            throw new ArgumentException("Property name cannot be empty", nameof(property));
        }

        if (node.IsNodeReady())
        {
            getNodeAndSetValue(path, property, value);
        }
        else
        {
            node.Ready += setter;
        }
    }
}
