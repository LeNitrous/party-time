using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Godot;

namespace Party;

public static class ArrayExtensions
{
    public static T GetRandom<T>(this IList<T> list)
    {
        return list[Random.Shared.Next(0, list.Count)];
    }
}
