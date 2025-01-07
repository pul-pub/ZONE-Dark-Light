using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIPDA : MonoBehaviour
{
    [SerializeField] private GameObject screen;
    [SerializeField] private QuestManager managerQuest;
    [Space]
    [SerializeField] private TextMeshProUGUI textTitelQuest;
    [SerializeField] private TextMeshProUGUI textNameQuest;
    [SerializeField] private TextMeshProUGUI textDiscriptionQuest;
    [SerializeField] private GameObject screenDiscription;
    [Space]
    [SerializeField] private RectTransform iconSaves;
    [Space]
    [SerializeField] private Object objQuest;
    [SerializeField] private RectTransform parentQuest;

    private float _timer = 0f;
    private Coroutine _saveAnim;
    public void OpenPDA()
    {
        UpdateScreenQuest();
    }

    public void Save()
    {
        if (_saveAnim == null)
        {
            SaveHeandler.SaveProgress(this);
            _saveAnim = StartCoroutine(SaveAnimation(2, 200));
        }  
    }
    public void ExitGame() => SceneManager.LoadScene(0, LoadSceneMode.Single);

    private void UpdateScreenQuest()
    {
        Transform[] _list = parentQuest.gameObject.GetComponentsInChildren<Transform>();

        for (int i = 0; i < _list.Length; i++)
        {
            GUIQuest _gui;

            if (_gui = _list[i].gameObject.GetComponent<GUIQuest>())
            {
                _list[i].gameObject.GetComponent<GUIQuest>().OnOpenDiscription -= OpenQuestDiscription;
                _list[i].gameObject.GetComponent<GUIQuest>().OnNow -= managerQuest.SetNowQuest;
                Destroy(_list[i].gameObject);
            }
        }

        Quest _nowQuest = managerQuest.GetNowQuests();
        List<Quest> _activQuest = managerQuest.GetActivQuests();

        for (int i = 0; i < _activQuest.Count; i++)
        {
            GameObject _gObj = Instantiate(objQuest, parentQuest) as GameObject;

            _gObj.GetComponent<GUIQuest>().Quest = _activQuest[i];
            _gObj.GetComponent<GUIQuest>().OnOpenDiscription += OpenQuestDiscription;
            _gObj.GetComponent<GUIQuest>().OnNow += managerQuest.SetNowQuest;

            _gObj.GetComponentInChildren<TextMeshProUGUI>().text = _activQuest[i].textTitell;
            if (_nowQuest == _activQuest[i])
                _gObj.GetComponentsInChildren<Image>()[1].color = Color.red;

            parentQuest.sizeDelta = new Vector2(0, 25 + 20 * i);
            _gObj.GetComponent<RectTransform>().localPosition -= new Vector3(0, 20 * i, 0);
        }
    }

    private void OpenQuestDiscription(Quest _q)
    {
        screenDiscription.SetActive(true);

        textTitelQuest.text = _q.textTitell;
        textNameQuest.text = "От: " + _q.NameFrom;
        textDiscriptionQuest.text = _q.textFullDiscription;
    }

    IEnumerator SaveAnimation(float _delay, float _speed)
    {
        _timer = _delay;
        iconSaves.gameObject.SetActive(true);

        while (_timer >= 0)
        {
            iconSaves.eulerAngles = new Vector3(0, 0, iconSaves.eulerAngles.z + (_speed * Time.deltaTime));
            _timer -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        iconSaves.gameObject.SetActive(false);

        _saveAnim = null;
    }
}
