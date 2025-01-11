using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIDetector : MonoBehaviour
{
    public event Action<GUISpinner, GUISpinner, float> OnSetChecking;

    [SerializeField] private GameObject objDetector;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI textLength;
    [SerializeField] private TextMeshProUGUI textAmplitude;
    [Header("Input")]
    [SerializeField] private GUISpinner spinerLength;
    [SerializeField] private GUISpinner spinerAmplitude;
    [Header("Grahp Settings")]
    [SerializeField] private int textureWidth = 400;
    [SerializeField] private int textureHeight = 400;
    [SerializeField] private int lineThickness = 3;
    [Space]
    [SerializeField] private Image imgGrahp;

    private DetectorObject _detector;

    private void OnEnable()
    {
        spinerAmplitude.OnChangeValue += OnChangeValue;
        spinerLength.OnChangeValue += OnChangeValue;
    }

    private void OnDisable()
    {
        spinerAmplitude.OnChangeValue -= OnChangeValue;
        spinerLength.OnChangeValue -= OnChangeValue;
    }

    public void Close() => SetActiv(null);

    public void SetActiv(DetectorObject _det)
    {
        _detector = _det;
        objDetector.SetActive(_det);
        OnSetChecking?.Invoke(
            spinerAmplitude,
            spinerLength,
            _detector ? _detector.RadiusCheck : -1);

        if (_detector)
            OnChangeValue(0f);
    }

    private void OnChangeValue(float value)
    {
        textAmplitude.text = Math.Round(spinerAmplitude.Value, 4, MidpointRounding.ToEven).ToString();
        textLength.text = Math.Round(spinerLength.Value, 4, MidpointRounding.ToEven).ToString();
        imgGrahp.sprite = GenerateSinWaveSprite(spinerAmplitude.Value, spinerLength.Value);
    }

    private Sprite GenerateSinWaveSprite(float _amplitude, float _period)
    {
        // Создаем текстуру
        Texture2D texture = new Texture2D(textureWidth, textureHeight);

        // Заполняем текстуру прозрачным цветом
        Color[] clearPixels = new Color[textureWidth * textureHeight];
        for (int i = 0; i < clearPixels.Length; i++)
        {
            clearPixels[i] = Color.clear;
        }
        texture.SetPixels(clearPixels);

        // Середина текстуры по вертикали
        float middleY = textureHeight / 2f;

        // Рисуем синусоиду
        for (int x = 0; x < textureWidth; x++)
        {
            // Вычисляем значение y для текущего x
            float normalizedX = (float)x / textureWidth;
            //float y = _amplitude * Mathf.Sin(2 * Mathf.PI * normalizedX * (1.0f / _period));
            float y =  Mathf.Sin(2 * Mathf.PI * normalizedX * (1.0f / _period));

            // Масштабируем y и смещаем его относительно середины текстуры
            float scaledY = y * 0.5f;
            int textureY = Mathf.RoundToInt(middleY + scaledY * middleY / _amplitude);

            // Рисуем линию с заданной толщиной
            for (int i = -lineThickness / 2; i <= lineThickness / 2; i++)
            {
                int currentY = textureY + i;
                if (currentY >= 0 && currentY < textureHeight)
                {
                    texture.SetPixel(x, currentY, new Color32(143, 226, 58, 255));
                }
            }
        }

        // Применяем изменения в текстуре
        texture.Apply();

        // Создаем спрайт из текстуры
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, textureWidth, textureHeight), new Vector2(0.5f, 0.5f));

        return sprite;
    }
}
