using UnityEngine;

public class DialogList : MonoBehaviour
{
    public string Name;
    public string Group;
    [Space]
    public NPC npc;
    public bool AutoOpenDialog = false;
    [Space]
    public Sprite face;
    public ArmorObject armor;
    public BackpackObject backpack;
    public LightObject lightObj;
    [Space]
    public Dialog startDialog;
}
