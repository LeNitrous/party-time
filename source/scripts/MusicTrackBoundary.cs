using Godot;
using NathanHoad;

namespace Party.Game;

public sealed partial class MusicTrackBoundary : Node
{
    [Export]
    public AudioStream Track { get; set; }

    [Export]
    public float Fade { get; set; } = 0.5f;

    [Export]
    public bool FromTheStart { get; set; } = true;

    public override void _Ready()
    {
        if (Track is null)
        {
            return;
        }

        var music = SoundManager.GetCurrentlyPlayingMusic();

        if (music.Count > 0 && music[0] == Track)
        {
            return;
        }

        if (FromTheStart)
        {
            SoundManager.PlayMusic(Track, 0, "Music");
        }
        else
        {
            if (Track is AudioStreamWav wav)
            {
                SoundManager.PlayMusicFromPosition(Track, wav.LoopBegin, Fade, "Music");
            }

            if (Track is AudioStreamMP3 mp3)
            {
                SoundManager.PlayMusicFromPosition(Track, (float)mp3.LoopOffset, Fade, "Music");
            }

            if (Track is AudioStreamOggVorbis ogg)
            {
                SoundManager.PlayMusicFromPosition(Track, (float)ogg.LoopOffset, Fade, "Music");
            }
        }
    }
}