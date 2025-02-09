using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Dialog")]
public class Dialog : ScriptableObject
{
    public int Id;
    public string NameNPC;
    [Header("������� �� �����")]
    public int GiveMoney = -1;
    public Item GiveItem;
    public int GiveCountItem;
    [Header("�����")]
    [TextArea]public string text;
    [Header("������")]
    public List<Answer> answers = new List<Answer>(3);
}
