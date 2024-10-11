using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Godot;

namespace Party.Game.Detection;

public unsafe struct PoseLandmarkerResult : IEnumerable<Vector3>
{
    public bool IsValid { get; }

    public readonly Vector3 this[PoseLandmark landmark]
    {
        get
        {
            fixed (float* ptr = pose)
            {
                return Unsafe.Read<Vector3>(Unsafe.Add<Vector3>(ptr, (int)landmark));
            }
        }
    }

    private fixed float pose[length];

    public PoseLandmarkerResult(ReadOnlySpan<Vector3> span)
    {
        if (span.Length != (int)PoseLandmark.Maximum)
        {
            throw new ArgumentException(null, nameof(span));
        }

        fixed (float* ptr = pose)
        {
            MemoryMarshal.Cast<Vector3, float>(span).CopyTo(new Span<float>(ptr, length));
        }

        IsValid = true;
    }

    public readonly IEnumerator<Vector3> GetEnumerator()
    {
        return new Enumerator(this);
    }

    readonly IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private const int length = (int)PoseLandmark.Maximum * 3;

    public struct Enumerator : IEnumerator<Vector3>
    {
        public readonly Vector3 Current => owner[(PoseLandmark)position];

        private int position = -1;
        private readonly PoseLandmarkerResult owner;

        public Enumerator(PoseLandmarkerResult owner)
        {
            this.owner = owner;
        }

        public bool MoveNext()
        {
            int next = position + 1;

            if (next >= (int)PoseLandmark.Maximum)
            {
                return false;
            }

            position = next;
            return true;
        }

        readonly void IEnumerator.Reset()
        {
        }

        readonly void IDisposable.Dispose()
        {
        }

        readonly object IEnumerator.Current => Current;
    }
}