using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class EntryMeta
{
    public Vector3 posFrom;
    public int locationFromID;
    public Scene sceneFrom;
    [Space]
    public Vector3 posTo;
    public int locationToID;
    public Scene sceneTo;
}
