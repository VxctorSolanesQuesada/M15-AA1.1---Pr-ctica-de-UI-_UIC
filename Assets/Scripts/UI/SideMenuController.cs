using UnityEngine;

public class SideMenuController : MonoBehaviour
{
    [Header("Panel Reference")]
    [SerializeField] private RectTransform menuPanel;

    [Header("Movable Content")]
    [SerializeField] private RectTransform menuContent;

    [Header("Resize Controller")]
    [SerializeField] private PanelResize panelResizer;

    [Header("Visual Elements")]
    [SerializeField] private GameObject iconOpen;
    [SerializeField] private GameObject iconClose;
    [SerializeField] private GameObject resizeIndicator;

    [Header("Slide Settings")]
    [SerializeField] private float openXPosition = 0f;
    [SerializeField] private float slideSpeed = 10f;

    private bool menuOpened = false;
    private Vector2 targetPosition;

    // ESTADO ORIGINAL DEL INSPECTOR
    private Vector2 initialPanelPosition;
    private Vector2 initialContentPosition;
    private Vector2 initialOffsetMin;
    private Vector2 initialOffsetMax;
    private float initialRightValue;

    private void Start()
    {
        // Guardamos EXACTAMENTE lo que hay en el inspector/al iniciar
        initialPanelPosition = menuPanel.anchoredPosition;
        initialContentPosition = menuContent.anchoredPosition;
        initialOffsetMin = menuPanel.offsetMin;
        initialOffsetMax = menuPanel.offsetMax;
        initialRightValue = -menuPanel.offsetMax.x;

        // Empezamos en la posición inicial
        targetPosition = initialPanelPosition;

        UpdateIcons();
    }

    private void Update()
    {
        menuPanel.anchoredPosition = Vector2.Lerp(
            menuPanel.anchoredPosition,
            targetPosition,
            slideSpeed * Time.unscaledDeltaTime
        );

        if (Vector2.Distance(menuPanel.anchoredPosition, targetPosition) < 0.1f)
        {
            menuPanel.anchoredPosition = targetPosition;
        }
    }

    public void ToggleMenu()
    {
        menuOpened = !menuOpened;

        if (menuOpened)
        {
            targetPosition = new Vector2(openXPosition, initialPanelPosition.y);
            Time.timeScale = 0f;
        }
        else
        {
            RestoreInitialState();
            Time.timeScale = 1f;
        }

        UpdateIcons();
    }

    private void RestoreInitialState()
    {
        // Restaurar exactamente el layout original
        menuPanel.offsetMin = initialOffsetMin;
        menuPanel.offsetMax = initialOffsetMax;
        menuPanel.anchoredPosition = initialPanelPosition;

        if (menuContent != null)
        {
            menuContent.anchoredPosition = initialContentPosition;
        }

        if (panelResizer != null)
        {
            panelResizer.SetRightValue(initialRightValue);
        }

        targetPosition = initialPanelPosition;
    }

    private void UpdateIcons()
    {
        if (iconOpen != null) iconOpen.SetActive(!menuOpened);
        if (resizeIndicator != null) resizeIndicator.SetActive(menuOpened);
        if (iconClose != null) iconClose.SetActive(menuOpened);
    }

    public void OpenOptionsTab()
    {
    }

    public void OpenContentTab()
    {
    }
}