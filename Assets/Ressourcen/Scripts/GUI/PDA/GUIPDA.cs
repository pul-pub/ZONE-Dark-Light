using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIPDA : MonoBehaviour
{
    [Header("----------  Modules  -----------")]
    [SerializeField] private QuestManager managerQuest;
    [Header("----------  Buttons  -----------")]
    [SerializeField] private GUIButton openPDA;
    [SerializeField] private GUIButton closePDA;
    [Header("----------  Screens  -----------")]
    [SerializeField] private RectTransform pda;
    [SerializeField] private Vector3 pdaPosT;
    [SerializeField] private Vector3 pdaPosF;
    [Space]
    [SerializeField] private RectTransform iconSaves;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private GameObject[] lights;
    [Space]
    [SerializeField] private Object objQuest;
    [SerializeField] private RectTransform parentQuest;
    [Header("--------  Description quest  ---------")]
    [SerializeField] private RectTransform screenDiscription;
    [SerializeField] private Vector3 posT;
    [SerializeField] private Vector3 posF;
    [Space]
    [SerializeField] private TextMeshProUGUI textTitelQuest;
    [SerializeField] private TextMeshProUGUI textNameQuest;
    [SerializeField] private TextMeshProUGUI textDiscriptionQuest;

    private bool _isOpen = false;
    private Coroutine _saveAnim;
    private Coroutine _pdaAnim;
    private Coroutine _lightsAnim;

    private void OnEnable()
    {
        openPDA.Click += OpenPDA;
        closePDA.Click += ClosePDA;
    }

    private void OnDisable()
    {
        openPDA.Click -= OpenPDA;
        closePDA.Click -= ClosePDA;
    }

    private void Update()
    {
        char[] arryHours = StaticValue.time[0].ToString().ToArray();
        char[] arryMinutes = StaticValue.time[1].ToString().ToArray();
        time.text = 
            (arryHours.Length == 1 ? "0" + arryHours[0] : StaticValue.time[0]) 
            + " : " + 
            (arryMinutes.Length == 1 ? "0" + arryMinutes[0] : StaticValue.time[1]);
    }

    public void OpenPDA(string _setting)
    {
        UpdateScreenQuest();
        if (_pdaAnim != null)
        {
            StopCoroutine( _pdaAnim );
            _pdaAnim = null;
        }
        _isOpen = true;
        _pdaAnim = StartCoroutine(AnimationPDA(pda, pdaPosT, 40));
        StartCoroutine(AnimationLights());
    }
    public void ClosePDA(string _setting)
    {
        if (_pdaAnim != null)
        {
            StopCoroutine(_pdaAnim);
            _pdaAnim = null;
        }
        _isOpen = false;
        _pdaAnim = StartCoroutine(AnimationPDA(pda, pdaPosF, 40));
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

    public void CloseQuestDiscription()
    {
        StopAllCoroutines();
        StartCoroutine(AnimationMove(screenDiscription, posF, 20, true));
    }

    private void OpenQuestDiscription(Quest _q)
    {
        StartCoroutine(AnimationMove(screenDiscription, posT, 20, true));

        textTitelQuest.text = _q.textTitell;
        textNameQuest.text = "От: " + _q.NameFrom;
        textDiscriptionQuest.text = _q.textFullDiscription;
    }

    IEnumerator SaveAnimation(float _delay, float _speed)
    {
        float _timer = 0;
        float _scale = 0;
        iconSaves.gameObject.SetActive(true);

        while (iconSaves.localScale.x < 1)
        {
            iconSaves.localScale = new Vector3(_scale, _scale, _scale);
            iconSaves.eulerAngles = new Vector3(0, 0, iconSaves.eulerAngles.z + (_speed * Time.deltaTime));
            _scale += 0.05f;
            yield return new WaitForEndOfFrame();
        }

        while (_timer <= _delay)
        {
            iconSaves.eulerAngles = new Vector3(0, 0, iconSaves.eulerAngles.z + (_speed * Time.deltaTime));
            _timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        while (iconSaves.localScale.x > 0)
        {
            iconSaves.localScale = new Vector3(_scale, _scale, _scale);
            iconSaves.eulerAngles = new Vector3(0, 0, iconSaves.eulerAngles.z + (_speed * Time.deltaTime));
            _scale -= 0.05f;
            yield return new WaitForEndOfFrame();
        }

        _saveAnim = null;
    }

    IEnumerator AnimationPDA(RectTransform _transform, Vector3 _target, float _speed)
    {
        while (!IsTargetValue(_transform.localPosition, _target))
        {
            _transform.localPosition = Vector3.MoveTowards(_transform.localPosition, _target, _speed * Time.deltaTime * 100);

            float progress = (Mathf.Abs(_transform.localPosition.y) - pdaPosF.y) / (pdaPosT.y - pdaPosF.y) - 1;
            float currentAngle = Mathf.LerpAngle(0f, 90f, progress); // Текущий угол
            _transform.localRotation = Quaternion.Euler(0, 0, currentAngle);

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator AnimationLights()
    {
        while (_isOpen)
        {
            lights[Random.Range(0, lights.Length)].SetActive(true);
            lights[Random.Range(0, lights.Length)].SetActive(false);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        }
    }

    IEnumerator AnimationMove(RectTransform _transform, Vector3 _target, float _speed, bool _animScale = false)
    {
        while (!IsTargetValue(_transform.localPosition, _target))
        {
            _transform.localPosition = Vector3.MoveTowards(_transform.localPosition, _target, _speed * Time.deltaTime * 100);

            if (_animScale)
                _transform.localScale = new Vector3(
                    -(Mathf.Abs(posF.y - _transform.localPosition.y)) / (posF.y - posT.y),
                    _transform.localScale.y, _transform.localScale.z);

            yield return new WaitForEndOfFrame();
        }
    }

    private float CalculateAngle(Vector3 position, Vector3 min, Vector3 max)
    {
        Vector3 normalizedPosition = new Vector3(
            (position.x - min.x) / (max.x - min.x),
            (position.y - min.y) / (max.y - min.y),
            (position.z - min.z) / (max.z - min.z)
        );

        float angleRadians = Mathf.Atan2(normalizedPosition.y, normalizedPosition.x);
        float angleDegrees = angleRadians * Mathf.Rad2Deg;
        angleDegrees = Mathf.Clamp(angleDegrees, 0, 90);

        return angleDegrees;
    }

    private bool IsTargetValue(Vector3 current, Vector3 target, float range = 0.1f)
    {
        Vector3 _v = current - target;
        return (Mathf.Abs(_v.x) <= range && Mathf.Abs(_v.y) <= range && Mathf.Abs(_v.z) <= range);
    }
}
