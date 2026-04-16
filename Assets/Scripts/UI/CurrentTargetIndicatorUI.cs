using UnityEngine;
using UnityEngine.UI;

public class CurrentTargetIndicatorUI : MonoBehaviour
{
    [Header("Referencias")]
    public Camera targetCamera;
    public RectTransform canvasRect;
    public RectTransform indicatorRect;
    public Image indicatorImage;
    public bool followPlayer;
    public Transform player;


    [Header("Alpha")]
    [Range(0f, 1f)] public float alphaVisible = 0.35f;
    [Range(0f, 1f)] public float alphaOccluded = 0.7f;
    [Range(0f, 1f)] public float alphaOffscreen = 1f;

    [Header("Detección de obstáculos")]
    public LayerMask occlusionMask = ~0;
    public float sphereCastRadius = 0.2f;

    private Transform target;

    void Reset()
    {
        indicatorRect = GetComponent<RectTransform>();
        indicatorImage = GetComponent<Image>();
    }

    void Update()
    {
        if (BallGameManager.instance == null) return;
        if (BallGameManager.instance.currentTarget == null) return;
        if (targetCamera == null || canvasRect == null || indicatorRect == null || indicatorImage == null) return;

        if (followPlayer)
        {
            target = player;
        }
        else
        {
            target = BallGameManager.instance.currentTarget;

        }

        Vector3 worldPos = target.position;

        Vector3 screenPos = targetCamera.WorldToScreenPoint(worldPos);
        bool behindCamera = screenPos.z < 0f;

        if (behindCamera)
        {
            screenPos *= -1f;
        }

        bool insideScreen =
            !behindCamera &&
            screenPos.x >= 0f && screenPos.x <= Screen.width &&
            screenPos.y >= 0f && screenPos.y <= Screen.height;

        Vector2 finalScreenPos = screenPos;

        if (!insideScreen)
        {
            Vector2 screenCenter = new Vector2(Screen.width, Screen.height) * 0.5f;
            Vector2 dir = ((Vector2)screenPos - screenCenter).normalized;

            float x = screenCenter.x;
            float y = screenCenter.y;

            float halfWidth = (Screen.width * 0.5f);
            float halfHeight = (Screen.height * 0.5f);

            float scaleX = dir.x != 0 ? halfWidth / Mathf.Abs(dir.x) : float.MaxValue;
            float scaleY = dir.y != 0 ? halfHeight / Mathf.Abs(dir.y) : float.MaxValue;
            float scale = Mathf.Min(scaleX, scaleY);

            finalScreenPos = screenCenter + dir * scale;
        }

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            finalScreenPos,
            null,
            out localPoint
        );

        indicatorRect.anchoredPosition = localPoint;

        float alpha;
        if (!insideScreen)
        {
            alpha = alphaOffscreen;
        }
        else
        {
            bool occluded = IsTargetOccluded(target);
            alpha = occluded ? alphaOccluded : alphaVisible;
        }

        SetAlpha(alpha);
    }

    bool IsTargetOccluded(Transform target)
    {
        Vector3 origin = targetCamera.transform.position;
        Vector3 targetPos = target.position;
        Vector3 dir = targetPos - origin;
        float distance = dir.magnitude;

        if (distance <= 0.001f) return false;

        dir.Normalize();

        if (Physics.SphereCast(origin, sphereCastRadius, dir, out RaycastHit hit, distance, occlusionMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform != target && !hit.transform.IsChildOf(target))
            {
                return true;
            }
        }

        return false;
    }

    void SetAlpha(float a)
    {
        Color c = indicatorImage.color;
        c.a = a;
        indicatorImage.color = c;
    }
}