using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Call")]
public class DialogCall : ScriptableObject
{
    public float TimeVivse = 2f;
    [Space]
    [TextArea] public string Dialog;
    [Space]
    public DialogCall NextDialog;
}
