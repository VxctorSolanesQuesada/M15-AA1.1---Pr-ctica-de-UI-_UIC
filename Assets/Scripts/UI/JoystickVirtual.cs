using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
public class JoystickVirtual : MonoBehaviour,
    IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public RectTransform joystick;
    public RectTransform joystickGhost;
    public RectTransform joystickParent;

    [SerializeField] private float maxRadius;


    public BallController controller;


    public Vector2 input;
    public bool reposition;
    private void Start()
    {

    }

    private void Update()
    {
        controller.Move(input);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (reposition)
        {
            joystickParent.position = eventData.position;
        }
        joystick.position = eventData.position;
        joystickGhost.position = eventData.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        joystick.position = eventData.position;
        joystickGhost.position = eventData.position;

        Vector3 dir = joystick.position - joystickParent.position;
        float distance = dir.magnitude;
        if (distance > maxRadius)
        {
            dir.Normalize();
            dir *= maxRadius;
            joystick.position = joystickParent.position + dir;
        }

        dir /= maxRadius;
        input = dir;

        
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        joystick.localPosition = Vector2.zero;
        joystickGhost.localPosition = Vector2.zero;
        input = Vector2.zero;

    }
}
