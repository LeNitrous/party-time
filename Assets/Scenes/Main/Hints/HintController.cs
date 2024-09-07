using UnityEngine;

public class HintController : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public string[] Hints
    {
        get => hints;
        set
        {
            hints = value;
            dirty = true;
        }
    }

    [SerializeField]
    private string[] hints;

    [SerializeField]
    private GameObject prefab;

    private bool dirty = true;

    private void Update()
    {
        if (dirty)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                var child = transform.GetChild(0).gameObject;

#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    DestroyImmediate(child);
                }
                else
#endif
                {
                    Destroy(child);
                }
            }

            if (prefab != null && (hints != null || hints.Length > 0))
            {
                for (int i = 0; i < hints.Length; i++)
                {
                    var hint = Instantiate(prefab, transform).GetComponent<Hint>();
                    hint.Action = hints[i];
                }
            }

            dirty = false;
        }
    }
}
