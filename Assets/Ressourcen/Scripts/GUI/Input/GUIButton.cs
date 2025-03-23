using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUIButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public event Action<string> Click;
    public event Action<bool> Clicking;

    public string toEndAction;

    public void OnPointerClick(PointerEventData eventData) => Click?.Invoke(toEndAction);

    public void OnPointerDown(PointerEventData eventData) => Clicking?.Invoke(true);

    public void OnPointerUp(PointerEventData eventData) => Clicking?.Invoke(false);
}
