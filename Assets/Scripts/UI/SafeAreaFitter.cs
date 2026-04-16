using UnityEngine;

public class SafeAreaFitter : MonoBehaviour
{
    [Header("Target UI")]
    [SerializeField] private RectTransform targetUI;

    [Header("Extra Padding (0 - 1)")]
    [SerializeField, Range(0f, 1f)] private float paddingX = 0f;
    [SerializeField, Range(0f, 1f)] private float paddingY = 0f;

    private void Start()
    {
        AdjustToSafeArea();
    }

    public void AdjustToSafeArea()
    {
        Rect safeZone = Screen.safeArea;

        float extraX = safeZone.width * paddingX;
        float extraY = safeZone.height * paddingY;

        Vector2 lowerBound = safeZone.position;
        Vector2 upperBound = safeZone.position + safeZone.size;

        lowerBound.x += extraX;
        upperBound.x -= extraX;

        lowerBound.y += extraY;
        upperBound.y -= extraY;

        lowerBound.x /= Screen.width;
        lowerBound.y /= Screen.height;

        upperBound.x /= Screen.width;
        upperBound.y /= Screen.height;

        targetUI.anchorMin = lowerBound;
        targetUI.anchorMax = upperBound;

        targetUI.offsetMin = Vector2.zero;
        targetUI.offsetMax = Vector2.zero;
    }
}