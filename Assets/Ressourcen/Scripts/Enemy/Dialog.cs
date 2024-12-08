using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Null", menuName = "Dialog")]
public class Dialog : ScriptableObject
{
    public int Id;
    public string NameNPC;
    [Header("Награда за квест")]
    public int getMoney = -1;
    public Item getItem;
    public int countItem;
    [Header("Текст")]
    public string text;
    [Header("Ответы")]
    public List<Answer> answers = new List<Answer>(3);
}
