using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUISpinner : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public event Action<float> OnChangeValue; 

    public float Value { get; private set; } = 0f;

    [SerializeField] private float stepValue = 0.1f;
    [SerializeField] private float stepRotation = 0.1f;
    [SerializeField] private float minValue = -100f;
    [SerializeField] private float startValue = 0f;
    [SerializeField] private float maxValue = 100f;
    [Space]
    [SerializeField] private Sprite[] spritsAnim;
    [Space]
    [SerializeField] private Image imgSpinner;

    private Vector2 currentPosTouch;
    private int indexSprite = 0;

    private void Awake()
    {
        Value = startValue;
    }

    

    public void OnBeginDrag(PointerEventData data) => currentPosTouch = data.position;

    public void OnDrag(PointerEventData data)
    {
        if (data.position.x - currentPosTouch.x <= -stepRotation)
        {
            if (Value - stepValue >= minValue)
            {
                Value -= stepValue;
                OnChangeValue?.Invoke(Value);
            }  
            indexSprite = indexSprite - 1 >= 0 ? indexSprite - 1 : spritsAnim.Length - 1;
            currentPosTouch = data.position;
        }
        else if (data.position.x - currentPosTouch.x >= stepRotation)
        {
            if (Value + stepValue <= maxValue)
            {   
                Value += stepValue;
                OnChangeValue?.Invoke(Value);
            }
            indexSprite = indexSprite + 1 < spritsAnim.Length ? indexSprite + 1 : 0;
            currentPosTouch = data.position;
        }

        imgSpinner.sprite = spritsAnim[indexSprite];
    }

    public void OnEndDrag(PointerEventData data) => currentPosTouch = Vector2.zero;
}
