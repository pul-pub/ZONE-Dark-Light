using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private DataBase data;
    [Space]
    [SerializeField] GUIHandler handler;
    [Space]
    [SerializeField] Object markeObject;
    [SerializeField] Transform parent;

    private Quest _nowQuest;
    private List<Quest> _quests = new();
    private List<Quest> _endedQuests = new();

    private List<RectTransform> _questMarkers = new();

    private void Awake()
    {
        if (SaveHeandler.SessionSave.idActivQuest != -1)
            _nowQuest = data.GetQuest(SaveHeandler.SessionSave.idActivQuest);

        foreach (int _id in SaveHeandler.SessionSave.idQuests)
            _quests.Add(data.GetQuest(_id));

        foreach (int _id in SaveHeandler.SessionSave.idEndingQuests)
            _endedQuests.Add(data.GetQuest(_id));
    }

    private void OnEnable()
    {
        SaveHeandler.OnSaveSession += SaveSession;
    }

    private void OnDisable()
    {
        SaveHeandler.OnSaveSession -= SaveSession;
    }

    private void Update()
    {
        if (_nowQuest != null)
        {

        }
    }

    public void SetNewQuest(Quest _q)
    {
        _nowQuest = _q;
        _quests.Add(_q);
        UpdateMarkers();
    }
    public void SetNowQuest(Quest _q)
    {
        _nowQuest = _q;
    }
    public void SetEndQuest(Quest _q)
    {
        if (_nowQuest == _q)
            _nowQuest = null;

        foreach (RectTransform _rt in _questMarkers)
        {
            if (_rt.position.x == _q.position.x)
            {
                _questMarkers.Remove(_rt);
                Destroy(_rt);
            }
        }

        _quests.Remove(_q);
        _endedQuests.Add(_q);
    }

    private void UpdateMarkers()
    {
        foreach (Quest _q in _quests)
        {
            bool _check = false;

            foreach (RectTransform _rt in _questMarkers)
                if (_rt.position.x == _q.position.x)
                    _check = true;

            if (!_check)
            {
                GameObject _gObj = Instantiate(markeObject, parent) as GameObject;

                RectTransform _rt = _gObj.GetComponent<RectTransform>();
                _rt.localPosition = new Vector3(_q.position.x, 2.5f);
                _questMarkers.Add(_rt);
            }
        }
    }

    private void SaveSession()
    {
        SaveHeandler.SessionSave.idActivQuest = _nowQuest != null ? _nowQuest.Id : -1;

        SaveHeandler.SessionSave.idQuests.Clear();
        SaveHeandler.SessionSave.idEndingQuests.Clear();

        foreach (Quest _q in _quests)
            SaveHeandler.SessionSave.idQuests.Add(_q.Id);

        foreach (Quest _q in _endedQuests)
            SaveHeandler.SessionSave.idEndingQuests.Add(_q.Id);
    }
}
