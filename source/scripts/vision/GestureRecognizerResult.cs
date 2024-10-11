using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;

namespace Party.Game.Detection;

public unsafe struct GestureRecognizerResult : IEnumerable<GestureRecognizerResult.Hand>
{
    public int Count { get; }

    public Hand this[int index]
    {
        get
        {
            fixed (byte* ptr = results)
            {
                return Unsafe.Read<Hand>(Unsafe.Add<Hand>(ptr, index));
            }
        }
    }

    private fixed byte results[length];

    public GestureRecognizerResult(ReadOnlySpan<Hand> hands)
    {
        fixed (byte* ptr = results)
        {
            MemoryMarshal.AsBytes(hands).CopyTo(new Span<byte>(ptr, length));
        }

        Count = hands.Length;
    }

    public readonly IEnumerator<Hand> GetEnumerator()
    {
        return new Enumerator(this);
    }

    readonly IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private const int length = 48;

    public readonly record struct Hand(Gesture Gesture, GestureHandedness Handedness, Rect2 Bounds);

    public struct Enumerator : IEnumerator<Hand>
    {
        public readonly Hand Current => owner[position];

        private int position = -1;
        private GestureRecognizerResult owner;

        public Enumerator(GestureRecognizerResult owner)
        {
            this.owner = owner;
        }

        public bool MoveNext()
        {
            int next = position + 1;

            if (next >= owner.Count)
            {
                return false;
            }

            position = next;
            return true;
        }

        void IEnumerator.Reset()
        {
        }

        void IDisposable.Dispose()
        {
        }

        readonly object IEnumerator.Current => Current;
    }
}
