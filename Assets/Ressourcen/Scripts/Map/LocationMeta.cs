using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class LocationMeta
{
    public int ID;
    public string Name;
    public bool isBidLocation = false;
    [Space]
    public Scene scene;
    [Space]
    public EntryMeta[] entryList;
}
