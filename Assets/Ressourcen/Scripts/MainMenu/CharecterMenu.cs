using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharecterMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject nullCharecter;
    [SerializeField] private Image[] imagesCharecter;
    [SerializeField] private TextMeshProUGUI nameCharecter;

    private SaveHeandler _saveHeandler;
    private int _numCharecter = 0;

    private void Awake()
    {
        _saveHeandler = new SaveHeandler();

        if (_saveHeandler.characters.Count > 0)
        {
            nullCharecter.SetActive(false);
            buttons[1].SetActive(true);

            
        }
    }

    private void Update()
    {
        
    }

    public void Next()
    {
        if (_numCharecter + 1 < _saveHeandler.characters.Count)
        {
            buttons[0].SetActive(true);
            buttons[1].SetActive(true);
        }
        else
        {
            nullCharecter.SetActive(true);
            buttons[1].SetActive(false);

            if (_saveHeandler.characters.Count > 0)
                buttons[0].SetActive(true);
            else
                buttons[0].SetActive(false);
        }
    }

    public void Back()
    {

    }
}
