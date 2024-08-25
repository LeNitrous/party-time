using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Scroller : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 1.0f)]
    private float speed = 0.1f;

    [SerializeField]
    private RawImage image;

    [SerializeField]
    private RectTransform rect;

    private void Awake()
    {
        if (image == null)
        {
            image = GetComponent<RawImage>();
        }

        if (rect == null)
        {
            rect = GetComponent<RectTransform>();
        }
    }

    private void Update()
    {
        if (image != null && rect != null)
        {
            Vector2 offset;

            if (Application.isPlaying)
            {
                offset = image.uvRect.position + (Vector2.one * -(speed * Time.deltaTime));
            }
            else
            {
                offset = Vector2.zero;
            }

            image.uvRect = new Rect(offset, rect.rect.size / new Vector2(image.mainTexture.width, image.mainTexture.height));
        }
    }
}
