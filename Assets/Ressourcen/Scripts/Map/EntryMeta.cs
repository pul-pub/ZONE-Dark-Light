using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Null", menuName = "EntryMeta")]
public class EntryMeta : ScriptableObject
{
    public Vector3 posFrom;
    public int locationFromID;
    public Scene sceneFrom;
    [Space]
    public Vector3 posTo;
    public int locationToID;
    public Scene sceneTo;
}
