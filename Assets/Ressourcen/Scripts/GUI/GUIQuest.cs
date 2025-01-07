using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUIQuest : MonoBehaviour, IPointerClickHandler
{
    public event Action<Quest> OnOpenDiscription;
    public event Action<Quest> OnNow;
    public Quest Quest;

    private int _countClick;

    public void OnPressButton() => OnNow?.Invoke(Quest);

    public void OnPointerClick(PointerEventData data)
    {
        if (_countClick >= 1)
        {
            OnOpenDiscription?.Invoke(Quest);
            _countClick = 0;
        }
        else
            _countClick++;
    }
}
