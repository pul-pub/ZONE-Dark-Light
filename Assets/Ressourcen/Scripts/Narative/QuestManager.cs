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

        handler.UpdateQuest(_nowQuest);
        UpdateMarkers();
    }

    private void OnEnable()
    {
        SaveHeandler.OnSaveSession += SaveSession;
    }

    private void OnDisable()
    {
        SaveHeandler.OnSaveSession -= SaveSession;
    }

    public Quest FindActivQuest(string _nameTo)
    {
        if (_nowQuest.NameTo == _nameTo)
            return _nowQuest;

        return _quests.Find(q => q.NameTo == _nameTo);
    }

    public void SetNewQuest(Quest _q)
    {
        _nowQuest = _q;
        _quests.Add(_q);
        UpdateMarkers();
        handler.UpdateQuest(_nowQuest);
    }
    public void SetNowQuest(Quest _q)
    {
        _nowQuest = _q;
        handler.UpdateQuest(_nowQuest);

        UpdateMarkers();
    }
    public void SetEndQuest(Quest _q)
    {
        if (_nowQuest == _q)
        {
            handler.UpdateQuest(_nowQuest);
            _nowQuest = null;
        }  

        _quests.Remove(_q);
        _endedQuests.Add(_q);
        UpdateMarkers();
    }

    public List<Quest> GetActivQuests() => _quests;
    public Quest GetNowQuests() => _nowQuest;

    private void UpdateMarkers()
    {
        foreach (Quest _q in _quests)
        {
            RectTransform _rt;

            if (!(_rt = _questMarkers.Find(__rt => __rt.position.x == _q.position.x)) && _q != _nowQuest)
            {
                GameObject _gObj = Instantiate(markeObject, parent) as GameObject;

                RectTransform _rTrans = _gObj.GetComponent<RectTransform>();
                _rTrans.localPosition = new Vector3(_q.position.x, 2.5f);
                _questMarkers.Add(_rTrans);
            }
            else if (_q == _nowQuest || _rt || _endedQuests.Find(__endedQuests => __endedQuests == _q))
            {
                _questMarkers.Remove(_rt);
                Destroy(_rt);
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
