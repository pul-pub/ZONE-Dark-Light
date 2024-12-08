using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Dialog")]
public class Dialog : ScriptableObject
{
    public int Id;
    public string NameNPC;
    [Header("������� �� �����")]
    public int getMoney = -1;
    public Item getItem;
    public int countItem;
    [Header("�����")]
    public string text;
    [Header("������")]
    public List<Answer> answers = new List<Answer>(3);
}
