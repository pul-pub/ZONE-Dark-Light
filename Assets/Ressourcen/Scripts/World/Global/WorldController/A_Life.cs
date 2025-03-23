using System.Collections.Generic;
using UnityEngine;

public class A_Life : MonoBehaviour
{
    [Header("Switch Object")]
    [SerializeField] private List<GameObject> listSwitchObject = new();
    [Header("Spot Objects")]
    [SerializeField] private List<CoreObject> listSpotObjects = new();

    private void Awake()
    {
        OnChangeObject(SaveHeandler.SessionNow.switcherObject);
    }

    private void OnEnable()
    {
        SaveHeandler.SessionNow.OnResetSwitchObject += OnChangeObject;
    }

    private void OnDisable()
    {
        SaveHeandler.SessionNow.OnResetSwitchObject -= OnChangeObject;
    }

    private void Update()
    {
        if (listSpotObjects.Count > 0 && !CheckStateSpot())
            SaveHeandler.SessionNow.SetSwitchObject("PriceNasos", true);
    }

    public bool CheckStateSpot()
    {
        foreach (CoreObject npc in listSpotObjects)
            if (npc != null && !npc.IsDide)
                return true;
        
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
