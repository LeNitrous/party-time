using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundTrackController : MonoBehaviour
{
    public static SoundTrackController Current { get; private set; }

    [SerializeField]
    private AudioMixer mixer;

    private int current = -1;
    private AudioMixerGroup[] groups;
    private AudioSource[] source;
    private Coroutine[] fading;
    private SoundTrackMetadata[] tracks;

    public void Play(SoundTrackMetadata track, bool fromLeadIn = false)
    {
        if (current >= 0)
        {
            if (tracks[current] == track)
            {
                return;
            }

            if (fading[current] != null)
            {
                StopCoroutine(fading[current]);
            }

            fading[current] = StartCoroutine(Fade(current, 1.0f, 1.0f, 0.0f));
        }

        int index = GetAvailableTrack();

        if (fading[index] != null)
        {
            StopCoroutine(fading[index]);
        }

        int start;

        if (fromLeadIn)
        {
            start = (int)(track.LeadInTime * track.Clip.frequency);
        }
        else
        {
            start = 0;
        }

        source[index].Stop();
        source[index].clip = track.Clip;
        source[index].Play();
        source[index].timeSamples = start;
        tracks[index] = track;
        fading[index] = StartCoroutine(Fade(index, 1.0f, 0.0f, 1.0f));
    }

    private int GetAvailableTrack()
    {
        return current = (current == 0 ? 1 : 0);
    }

    private IEnumerator Fade(int index, float duration, float target)
    {
        return Fade(index, duration, -1.0f, target);
    }

    private IEnumerator Fade(int index, float duration, float start, float target)
    {
        if (mixer == null)
        {
            yield break;
        }

        if (index < 0 || index >= groups.Length)
        {
            yield break;
        }

        string parameter = string.Format(parameterFormat, index);

        float current;
        float elapsed = 0.0f;

        if (start >= 0.0f)
        {
            if (!mixer.SetFloat(parameter, current = start))
            {
                yield break;
            }
        }
        else
        {
            if (!mixer.GetFloat(parameter, out current))
            {
                current = 0.0f;
            }
            else
            {
                current = Mathf.Pow(10, current / 20.0f);
            }
        }

        target = Mathf.Clamp(target, 0.0001f, 1.0f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            if (!mixer.SetFloat(parameter, Mathf.Log10(Mathf.Lerp(current, target, elapsed / duration)) * 20))
            {
                yield break;
            }

            yield return null;
        }

        if (fading[index] != null)
        {
            fading[index] = null;
        }

        yield break;
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
        groups = mixer.FindMatchingGroups("Music/Track");
        source = new AudioSource[groups.Length];
        tracks = new SoundTrackMetadata[groups.Length];
        fading = new Coroutine[groups.Length];

        for (int i = 0; i < groups.Length; i++)
        {
            source[i] = gameObject.AddComponent<AudioSource>();
            source[i].outputAudioMixerGroup = groups[i];
        }
    }

    private void Update()
    {
        for (int i = 0; i < tracks.Length; i++)
        {
            if (tracks[i] == null)
            {
                continue;
            }

            if (tracks[i].LoopStartTime > tracks[i].LoopEndTime)
            {
                continue;
            }

            var audio = source[i];
            var track = tracks[i];

            if (audio.timeSamples >= (int)(track.LoopEndTime * audio.clip.frequency))
            {
                audio.timeSamples -= (int)(track.LoopEndTime * audio.clip.frequency) - (int)(track.LoopStartTime * audio.clip.frequency);
            }
        }
    }

    private const string parameterFormat = "music.track.{0}";
}
