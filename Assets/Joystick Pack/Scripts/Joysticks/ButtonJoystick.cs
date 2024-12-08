using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonJoystick : Joystick
{
    protected override void Start()
    {
        base.Start();
        board.enabled = false;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        board.enabled = true;
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        board.enabled = false;
        base.OnPointerUp(eventData);
    }
}
