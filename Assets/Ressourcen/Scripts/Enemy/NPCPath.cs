using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "NPC Path")]
public class NPCPath : ScriptableObject
{
    public float TimeStartPause = 0;
    [Space]
    public float PosX = 0;
    [Space]
    public float TimeEndPause = 0;
}
