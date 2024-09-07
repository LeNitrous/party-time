using System;
using UnityEngine;

public class HintController : MonoBehaviour
{
    [Serializable]
    public struct Control
    {
        /// <summary>
        /// The label displayed.
        /// </summary>
        public string Label;

        /// <summary>
        /// The icon names displayed.
        /// </summary>
        public string Action;

        public Control(string label, string action)
        {
            Label = label;
            Action = action;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public Control[] Hints
    {
        get => hints;
        set
        {
            hints = value;
            dirty = true;
        }
    }

    [SerializeField]
    private Control[] hints;

    [SerializeField]
    private GameObject prefab;

    private bool dirty = true;

    private void Update()
    {
        if (dirty)
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }

            if (prefab != null && (hints != null || hints.Length > 0))
            {
                for (int i = 0; i < hints.Length; i++)
                {
                    var hint = Instantiate(prefab, transform).GetComponent<Hint>();
                    hint.Label = hints[i].Label;
                    hint.Action = hints[i].Action;
                }
            }

            dirty = false;
        }
    }
}
