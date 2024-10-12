using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Party;

public static class EnumerableExtensions
{
    public static T GetRandom<T>(this IReadOnlyList<T> list)
    {
        return list[Random.Shared.Next(0, list.Count)];
    }

    public static T GetRandomWeighted<T>(this IEnumerable<T> enumerable, Func<T, double> weightSelector)
    {
        double totalWeight = enumerable.Sum(weightSelector);
        double itemWeightIndex = Random.Shared.NextDouble() * totalWeight;
        double currentWeighted = 0;

        foreach (var item in enumerable.Select(i => new ValueTuple<T, double>(i, weightSelector(i))))
        {
            currentWeighted += item.Item2;

            if (currentWeighted >= itemWeightIndex)
            {
                return item.Item1;
            }
        }

        return default;
    }
}

public static class DirAccessExtensions
{
    public static IEnumerable<string> Enumerate(this DirAccess dir, bool includeDirectories = false)
    {
        string item = string.Empty;

        dir.ListDirBegin();

        do
        {
            item = dir.GetNext();

            if (dir.CurrentIsDir() && includeDirectories)
            {
                yield return item;
            }
            else
            {
                continue;
            }

            yield return item;
        }
        while (!string.IsNullOrEmpty(item));
    }
}
