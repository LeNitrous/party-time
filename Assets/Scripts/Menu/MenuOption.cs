using System;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour, ISubmitHandler, ISelectHandler, IDeselectHandler, IMoveHandler, IPointerClickHandler
{
    public string[] Options
    {
        get => options;
        set
        {
            options = value;
            UpdateVisual();
        }
    }

    public string Tooltip => tooltip;

    public bool Interactive => (onSubmit?.GetPersistentEventCount() ?? 0) > 0;

    [BoxGroup("Background")]
    [SerializeField]
    private Image background;

    [BoxGroup("Labels")]
    [SerializeField]
    private TextMeshProUGUI label;

    [BoxGroup("Labels")]
    [SerializeField]
    private TextMeshProUGUI value;

    [ResizableTextArea]
    [SerializeField]
    private string tooltip;

    [BoxGroup("Buttons")]
    [Label("Left")]
    [SerializeField]
    private GameObject buttonL;

    [BoxGroup("Buttons")]
    [Label("Right")]
    [SerializeField]
    private GameObject buttonR;

    [Foldout("Options")]
    [ReorderableList]
    [SerializeField]
    private string[] options;

    [Foldout("Events")]
    [SerializeField]
    private UnityEvent onSubmit;

    [Foldout("Events")]
    [SerializeField]
    private MenuOptionValueChanged onChange;

    private int selected;
    private bool isActive;
    private MenuScreen controller;
    private GraphicRaycaster raycast;
    private List<RaycastResult> results;

    private void Start()
    {
        if (raycast == null)
        {
            raycast = GetComponentInParent<GraphicRaycaster>();
        }

        if (background == null)
        {
            background = GetComponent<Image>();
        }

        if (controller == null)
        {
            controller = GetComponentInParent<MenuScreen>();
        }
    }

    private void OnValidate()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (isActive)
        {
            label.color = Color.white;
            value.color = Color.white;
            background.color = new Color32(16, 16, 16, 255);
        }
        else
        {
            label.color = new Color32(16, 16, 16, 255);
            value.color = new Color32(16, 16, 16, 255);
            background.color = new Color32(0, 0, 0, 0);
        }

        if (options != null && options.Length > 0)
        {
            if (!value.gameObject.activeSelf)
            {
                value.gameObject.SetActive(true);
            }

            value.text = options[selected];
            buttonL.SetActive(isActive);
            buttonR.SetActive(isActive);
        }
        else
        {
            if (buttonL.activeSelf)
            {
                buttonL.SetActive(false);
            }

            if (buttonR.activeSelf)
            {
                buttonR.SetActive(false);
            }

            value.gameObject.SetActive(false);
        }
    }

    public void Next()
    {
        if (options == null || options.Length == 0)
        {
            return;
        }

        int next = selected + 1;

        if (next > options.Length -1)
        {
            next = 0;
        }

        selected = next;
        UpdateVisual();
    }

    public void Previous()
    {
        if (options == null || options.Length == 0)
        {
            return;
        }

        int next = selected - 1;

        if (next < 0)
        {
            next = options.Length - 1;
        }

        selected = next;
        UpdateVisual();
    }

    void ISubmitHandler.OnSubmit(BaseEventData eventData)
    {
        onSubmit?.Invoke();
    }

    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        if (!isActive)
        {
            isActive = true;
            UpdateVisual();
        }
    }

    void IDeselectHandler.OnDeselect(BaseEventData eventData)
    {
        if (isActive)
        {
            isActive = false;
            UpdateVisual();
        }
    }

    void IMoveHandler.OnMove(AxisEventData eventData)
    {
        switch (eventData.moveDir)
        {
            case MoveDirection.Left:
                Previous();
                break;

            case MoveDirection.Right:
                Next();
                break;

            case MoveDirection.Up when controller != null:
                controller.Previous();
                break;

            case MoveDirection.Down when controller != null:
                controller.Next();
                break;
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        results ??= new List<RaycastResult>();
        results.Clear();
        raycast.Raycast(eventData, results);

        var hit = results[0].gameObject;

        if (hit == buttonL)
        {
            Previous();
        }

        if (hit == buttonR)
        {
            Next();
        }
    }
}

[Serializable]
public class MenuOptionValueChanged : UnityEvent<int>
{
}
