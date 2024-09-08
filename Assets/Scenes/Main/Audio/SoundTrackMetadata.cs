using UnityEngine;

[CreateAssetMenu(fileName= "Track Metadata", menuName = "Audio/Track Metadata")]
public class SoundTrackMetadata : ScriptableObject
{
    public AudioClip Clip;

    public float LoopStartTime;

    public float LoopEndTime;

    public float LeadInTime;
}
