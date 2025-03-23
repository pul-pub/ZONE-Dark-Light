using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUIQuest : MonoBehaviour, IPointerClickHandler
{
    public event Action<Quest> OnOpenDiscription;
    public event Action<Quest> OnNow;
    public Quest Quest;

    private int _countClick;
    private float _timer;

    private void Update()
    {
        if (_timer >= 0)
            _timer -= Time.deltaTime;
    }

    public void OnPressButton() => OnNow?.Invoke(Quest);

    public void OnPointerClick(PointerEventData data)
    {
        if (_countClick < 1 && _timer <= 0)
        {
            _countClick++;
            _timer = 0.5f;
        }
        else if (_countClick == 1 && _timer > 0)
        {
            OnOpenDiscription.Invoke(Quest);
            _countClick = 0;
        }
        else
            _countClick = 0;
    }
}
