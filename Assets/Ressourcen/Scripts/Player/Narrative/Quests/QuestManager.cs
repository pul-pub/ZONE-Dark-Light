using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public event System.Action<Quest> UpdateQuestTitles;

    public Quest NowQuest { get; private set; }

    [SerializeField] private DataBase data;
    [Header("---------  Marker  ---------")]
    [SerializeField] private Object markeObject;
    [SerializeField] private Transform parent;

    private List<Quest> _quests = new();
    private List<Quest> _endedQuests = new();

    private List<GameObject> _questMarkers = new();

    public List<Quest> GetActivQuests() => _quests;
    public Quest GetNowQuests() => NowQuest;
    public Quest FindActivQuest(string _nameTo)
    {
        if (NowQuest)
        {
            if (NowQuest.NameTo == _nameTo)
                return NowQuest;

            return _quests.Find(q => q.NameTo == _nameTo);
        }

        return null;
    }

    public void AddQuest(Quest _q)
    {
        _quests.Add(_q);
        SetNowQuest(_q);
    }
    public void SetNowQuest(Quest _q)
    {
        NowQuest = _q;
        UpdateQuestTitles?.Invoke(NowQuest);

        UpdateMarkers();
    }
    public void SetEndQuest(Quest _q)
    {
        if (NowQuest == _q)
        {
            NowQuest = null;
            UpdateQuestTitles?.Invoke(NowQuest);
        }
        
        _quests.Remove(_q);
        _endedQuests.Add(_q);
        UpdateMarkers();
    }

    private void UpdateMarkers()
    {
        foreach (Quest _q in _quests)
        {
            GameObject _gObj;
            if (!_questMarkers.Find(_obj => _obj.name == _q.Id.ToString()) && _q != NowQuest)
            {
                _gObj = Instantiate(markeObject, parent) as GameObject;
                _gObj.name = _q.Id.ToString();

                _gObj.GetComponent<RectTransform>().localPosition = new Vector3(_q.position.x, (_q.position.y == 0 ? 2.5f : _q.position.y), 0);
                _questMarkers.Add(_gObj);
            }
            else if (_q == NowQuest && (_gObj = _questMarkers.Find(_obj => _obj.name == _q.Id.ToString())))
            {
                _questMarkers.Remove(_gObj);
                Destroy(_gObj);
            }
        }
    }

    public void Save()
    {
        SaveHeandler.SessionNow.idActivQuest = NowQuest != null ? NowQuest.Id : -1;

        SaveHeandler.SessionNow.idQuests.Clear();
        SaveHeandler.SessionNow.idEndingQuests.Clear();

        foreach (Quest _q in _quests)
            SaveHeandler.SessionNow.idQuests.Add(_q.Id);

        foreach (Quest _q in _endedQuests)
            SaveHeandler.SessionNow.idEndingQuests.Add(_q.Id);
    }
    public void Load()
    {
        if (SaveHeandler.SessionNow.idActivQuest != -1)
            NowQuest = data.GetQuest(SaveHeandler.SessionNow.idActivQuest);

        foreach (int _id in SaveHeandler.SessionNow.idQuests)
            _quests.Add(data.GetQuest(_id));

        foreach (int _id in SaveHeandler.SessionNow.idEndingQuests)
            _endedQuests.Add(data.GetQuest(_id));

        UpdateQuestTitles?.Invoke(NowQuest);
        UpdateMarkers();
    }
}
