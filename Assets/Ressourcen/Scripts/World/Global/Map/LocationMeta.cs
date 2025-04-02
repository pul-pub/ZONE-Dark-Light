using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LocationMeta
{
    public int ID;
    public string Name;
    [Space]
    public List<EntryMeta> Connections;
    public Sprite BG;
}
