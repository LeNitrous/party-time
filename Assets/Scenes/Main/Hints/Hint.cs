using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public class Hint : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public string Action
    {
        get => actionName;
        set
        {
            actionName = value;
            OnInputSchemeChanged(input);
        }
    }

    [SerializeField]
    private string actionName;

    private bool invalidated;
    private Transform icons;
    private SpriteAtlas atlas;
    private PlayerInput input;
    private TextMeshProUGUI label;

    private void OnInputSchemeChanged(PlayerInput input)
    {
        invalidated = true;
    }

    private GameObject CreateIconObject(string name)
    {
        var icon = new GameObject(name);

        //var image = icon.AddComponent<Image>();
        //image.sprite = atlas.GetSprite(name);

        //var layout = icon.AddComponent<LayoutElement>();
        //layout.preferredWidth = 30;
        //layout.preferredHeight = 30;

        return icon;
    }

    private void Start()
    {
        icons = transform.Find("Icons");
        label = transform.Find("Label").GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (input == null)
        {
            input = FindObjectOfType<PlayerInput>();
        }

        input.onControlsChanged += OnInputSchemeChanged;
        OnInputSchemeChanged(input);
    }

    private void OnDisable()
    {
        input.onControlsChanged -= OnInputSchemeChanged;
    }

    private void Update()
    {
        if (invalidated)
        {
            var action = input.actions.FindAction(actionName);

            if (icons != null)
            {
                for (int i = icons.childCount; i > 0; i--)
                {
                    Destroy(icons.GetChild(0).gameObject);
                }
            }

            if (action != null)
            {
                int bindingIndex = action.GetBindingIndex(input.currentControlScheme);
                var binding = action.bindings[bindingIndex];

                if (binding.isComposite || binding.isPartOfComposite)
                {
                    var bindingGroup = InputBinding.MaskByGroup(input.currentControlScheme);

                    for (; bindingIndex < action.bindings.Count; bindingIndex++)
                    {
                        binding = action.bindings[bindingIndex];

                        if (!bindingGroup.Matches(binding))
                        {
                            break;
                        }

                        Instantiate(CreateIconObject(binding.ToDisplayString()), icons);
                    }
                }
                else
                {
                    Instantiate(CreateIconObject(binding.ToDisplayString()), icons);
                }

                label.text = action.name;
            }
            else
            {
                label.text = unknown;
            }

            invalidated = false;
        }
    }

    private const string unknown = "<unknown>";
}
