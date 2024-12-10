using System;
using UnityEngine;

public enum TypeDescription { NextDialog, Quest, Buy, Sale, Exit };

[Serializable]
public class Answer
{
    [TextArea] public string text;
    public Dialog nextDialog;
    //quest
    public TypeDescription typeDescriptions;
}
