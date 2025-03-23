using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class LocationMeta
{
    public int ID;
    public string Name;
    [Space]
    public List<EntryMeta> Connections;
    public Sprite BG;
}
