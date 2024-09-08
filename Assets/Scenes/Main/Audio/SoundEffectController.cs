using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

public class SoundEffectController : MonoBehaviour
{
    /// <summary>
    /// The current sound effect controller instance.
    /// </summary>
    public static SoundEffectController Current { get; private set; }

    [SerializeField, Tooltip("The maximum amount of sound effects that can be played in the scene.")]
    private int maximum;

    [SerializeField, Tooltip("The audio mixer group the sound effect output will route to.")]
    private AudioMixerGroup group;

    private ObjectPool<AudioSource> pool;

    /// <summary>
    /// Rents a <see cref="AudioSource"/>.
    /// </summary>
    /// <param name="source">The audio source rented.</param>
    /// <returns>A disposable when disposed, returns the rented audio source.</returns>
    public IDisposable Rent(out AudioSource source)
    {
        if (pool == null)
        {
            source = null;
            return null;
        }

        return pool.Get(out source);
    }

    private AudioSource Create()
    {
        return gameObject.AddComponent<AudioSource>();
    }

    private void OnPoolRent(AudioSource source)
    {
        source.volume = 1.0f;
        source.pitch = 1.0f;
        source.loop = false;
        source.playOnAwake = false;
        source.outputAudioMixerGroup = group;
    }

    private void OnPoolDestroy(AudioSource source)
    {
        Destroy(source);
    }

    private void Awake()
    {
        if (Current != null && Current != this)
        {
            Destroy(this);
        }
        else
        {
            Current = this;
        }
    }

    private void Start()
    {
        if (maximum > 0)
        {
            pool = new ObjectPool<AudioSource>(Create, OnPoolRent, null, OnPoolDestroy, maxSize: maximum);
        }
    }
}
