using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Quest")]
public class Quest : ScriptableObject
{
    public int Id;
    [Space]
    public string NameFrom;
    public string NameTo;
    [Space]
    public Vector2 position;
    public int idScene;
    [Header("Text")]
    public string textTitell;
    public string textDiscription;
    public string textFullDiscription;
    [Space]
    public Dialog startDialog;
}
