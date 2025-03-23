using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Dialog")]
public class Dialog : ScriptableObject
{
    public int Id;
    public string NameNPC;
    [Header("Награда за квест")]
    public bool questReward = false;
    public int GiveMoney = -1;
    public List<StaticItem> GiveItems = new List<StaticItem>();
    [Header("Текст")]
    [TextArea]public string text;
    [Header("Ответы")]
    public List<Answer> answers = new List<Answer>(3);
}
