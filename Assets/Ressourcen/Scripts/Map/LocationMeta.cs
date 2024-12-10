using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class LocationMeta
{
    public int ID;
    public string Name;
    [Space]
    public Scene scene;
    [Space]
    public EntryMeta[] entryList;
}
