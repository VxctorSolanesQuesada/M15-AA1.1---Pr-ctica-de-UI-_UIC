using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PanelResize : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [Header("References")]
    [SerializeField] private RectTransform targetPanel;
    [SerializeField] private RectTransform innerContent;

    [Header("Resize Range")]
    [SerializeField] private float minimumWidth = 100f;
    [SerializeField] private float maximumWidth = 600f;

    [Header("Layout Spacing")]
    [SerializeField] private float minimumSpacing = 5f;
    [SerializeField] private float maximumSpacing = 50f;

    private HorizontalOrVerticalLayoutGroup[] cachedLayouts;
    private Vector2 dragStartPosition;
    private float initialPanelRight;

    private void Awake()
    {
        if (targetPanel != null)
        {
            cachedLayouts = targetPanel.GetComponentsInChildren<HorizontalOrVerticalLayoutGroup>(true);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartPosition = eventData.position;
        initialPanelRight = GetCurrentRightValue();
    }

    public void OnDrag(PointerEventData eventData)
    {
        float horizontalDrag = eventData.position.x - dragStartPosition.x;
        float desiredRight = Mathf.Clamp(initialPanelRight - horizontalDrag, minimumWidth, maximumWidth);

        ApplyPanelWidth(desiredRight);
        KeepContentAligned();
        UpdateLayoutSpacing(desiredRight);
    }

    public void SetRightValue(float newRightValue)
    {
        initialPanelRight = newRightValue;
        ApplyMaxSpacingToLayouts();
    }

    private float GetCurrentRightValue()
    {
        return -targetPanel.offsetMax.x;
    }

    private void ApplyPanelWidth(float rightValue)
    {
        Vector2 currentOffsetMax = targetPanel.offsetMax;
        currentOffsetMax.x = -rightValue;
        targetPanel.offsetMax = currentOffsetMax;
    }

    private void KeepContentAligned()
    {
        if (innerContent == null) return;

        Vector2 currentPosition = innerContent.anchoredPosition;
        innerContent.anchoredPosition = new Vector2(0f, currentPosition.y);
    }

    private void UpdateLayoutSpacing(float currentRight)
    {
        if (cachedLayouts == null || cachedLayouts.Length == 0) return;

        float normalizedValue = Mathf.InverseLerp(minimumWidth, maximumWidth, currentRight);
        float newSpacing = Mathf.Lerp(minimumSpacing, maximumSpacing, normalizedValue);

        for (int i = 0; i < cachedLayouts.Length; i++)
        {
            cachedLayouts[i].spacing = newSpacing;
        }
    }

    private void ApplyMaxSpacingToLayouts()
    {
        if (cachedLayouts == null || cachedLayouts.Length == 0) return;

        for (int i = 0; i < cachedLayouts.Length; i++)
        {
            cachedLayouts[i].spacing = maximumSpacing;
        }
    }
}