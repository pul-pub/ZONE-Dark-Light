using System.Collections.Generic;
using UnityEngine;

public class A_Life : MonoBehaviour
{
    [Header("Switch Object")]
    [SerializeField] private List<GameObject> listSwitchObject = new();
    [Header("Spot Objects")]
    [SerializeField] private List<NPC> listSpotObjects = new();

    private void Awake()
    {
        OnChangeObject(SaveHeandler.SessionSave.switcherObject);
    }

    private void OnEnable()
    {
        SaveHeandler.SessionSave.OnResetSwitchObject += OnChangeObject;
    }

    private void OnDisable()
    {
        SaveHeandler.SessionSave.OnResetSwitchObject -= OnChangeObject;
    }

    private void Update()
    {
        if (listSpotObjects.Count > 0 && !CheckStateSpot())
            SaveHeandler.SessionSave.SetSwitchObject("PriceNasos", true);
    }

    public bool CheckStateSpot()
    {
        foreach (NPC npc in listSpotObjects)
            if (npc != null && npc.backpack == null)
                return true;
        Debug.Log("f");
        return false;
    }

    public void OnChangeObject(Dictionary<string, bool> _objects)
    {
        foreach (string _key in _objects.Keys)
        {
            GameObject _gObj;

            if (_gObj = listSwitchObject.Find(_obj => _obj.name == _key))
                if (_gObj.activeSelf != _objects[_key])
                    _gObj.SetActive(_objects[_key]);
        }
    }
}
