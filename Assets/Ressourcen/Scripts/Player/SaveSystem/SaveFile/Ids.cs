using MessagePack;
using MessagePack.Unity;
using System.Collections.Generic;
using UnityEngine;


[MessagePackObject]
public class Ids 
{
    [Key(0)]
    public Dictionary<string, Character> keysCharecters = new Dictionary<string, Character>();
}
