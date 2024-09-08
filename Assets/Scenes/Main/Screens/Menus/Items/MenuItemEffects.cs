using UnityEngine;
using UnityEngine.EventSystems;

public class MenuItemEffects : MonoBehaviour, ISubmitHandler, IPointerClickHandler
{
    [SerializeField]
    private AudioClip[] samples;

    private void Play()
    {
        if (samples == null || samples.Length == 0)
        {
            return;
        }

        using (SoundEffectController.Current.Rent(out var source))
        {
            source.pitch = Random.Range(0.6f, 1.2f);
            source.PlayOneShot(samples[Random.Range(0, samples.Length - 1)]);
        }
    }

    void ISubmitHandler.OnSubmit(BaseEventData eventData)
    {
        Play();
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        Play();
    }
}
