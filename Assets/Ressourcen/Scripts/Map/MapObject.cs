using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Map")]
public class MapObject : ScriptableObject
{
    public LocationMeta[] location;
}
