using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ScreenBackgroundController : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public Color Color
    {
        get => color;
        set
        {
            if (color.Equals(value))
            {
                return;
            }

            color = value;
            SetAccent(color);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool ShowPattern
    {
        get => showPattern;
        set
        {
            if (showPattern == value)
            {
                return;
            }

            showPattern = value;
            SetEffects(showPattern, showEffects);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public bool ShowEffects
    {
        get => showEffects;
        set
        {
            if (showEffects == value)
            {
                return;
            }

            showEffects = value;
            SetEffects(showPattern, showEffects);
        }
    }

    [SerializeField]
    private bool showPattern;

    [SerializeField]
    private bool showEffects;

    [SerializeField]
    private Color color = Color.white;

    [SerializeField]
    private float speed = 0.1f;

    [SerializeField]
    private float scale = 1.0f;

    private Image back;
    private Image solid;
    private Image edger;
    private RawImage scrolling;

    private void SetEffects(bool pattern, bool effects)
    {
        if (solid != null)
        {
            solid.gameObject.SetActive(effects);
        }

        if (edger != null)
        {
            edger.gameObject.SetActive(effects);
        }

        if (scrolling != null)
        {
            scrolling.gameObject.SetActive(pattern);
        }
    }

    private void SetPattern(Texture texture)
    {
        if (scrolling != null)
        {
            scrolling.texture = texture;
        }
    }

    private void SetAccent(Color color)
    {
        var gray = new Color(color.r * 1.05f, color.g * 1.05f, color.b * 1.05f, color.a);

        if (back != null)
        {
            back.color = color;
        }

        if (solid != null)
        {
            solid.color = gray;
        }

        if (edger != null)
        {
            edger.color = gray;
        }

        if (scrolling != null)
        {
            scrolling.color = gray;
        }
    }

    private void Start()
    {
        back = GetComponent<Image>();
        solid = transform.Find("Halftone/Solid").GetComponent<Image>();
        edger = transform.Find("Halftone/Edger").GetComponent<Image>();
        scrolling = transform.Find("Scrolling").GetComponent<RawImage>();
    }

    private void Update()
    {
        Vector2 offset;

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            offset = Vector2.zero;
        }
        else
#endif
        {
            offset = scrolling.uvRect.position + (Vector2.one * -(speed * Time.deltaTime));
        }

        scrolling.uvRect = new Rect(offset, ((RectTransform)transform).rect.size / new Vector2(scrolling.texture.width, scrolling.texture.height) * scale);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        SetAccent(Color);
        SetEffects(ShowPattern, ShowEffects);
    }
#endif
}
