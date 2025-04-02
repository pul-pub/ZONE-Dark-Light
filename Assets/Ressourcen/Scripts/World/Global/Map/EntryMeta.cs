using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "EntryMeta")]
public class EntryMeta : ScriptableObject
{
    public Vector3 posFrom;
    public int locationFromID;
    [Space]
    public Vector3 posTo;
    public int locationToID;
}
