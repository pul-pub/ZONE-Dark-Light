using System.Collections.Generic;
using UnityEngine;

public class A_Life : MonoBehaviour
{
    [Header("Switch Object")]
    [SerializeField] private List<GameObject> listSwitchObject = new();

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
