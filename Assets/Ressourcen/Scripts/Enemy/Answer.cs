using System;

public enum TypeDescription { NextDialog, Quest, Buy, Sale, Exit };

[Serializable]
public class Answer
{
    public string text;
    public Dialog nextDialog;
    //quest
    public TypeDescription typeDescriptions;
}
