using MessagePack;
using System.Collections.Generic;


[MessagePackObject]
public class Ids 
{
    [Key(0)]
    public Dictionary<string, Character> keysCharecters = new Dictionary<string, Character>();
}
