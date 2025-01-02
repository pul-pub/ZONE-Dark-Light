using System;
using UnityEngine;

public enum TypeDescription { NextDialog, Quest, Buy, Sale, Exit, WalkTo };

[Serializable]
public class Answer
{
    [TextArea] public string text;
    public Dialog nextDialog;
    public EntryMeta metaEntry;
    public Quest quest;
    public TypeDescription typeDescriptions;
}
