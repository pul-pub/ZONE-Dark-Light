using System.Collections.Generic;
using UnityEngine;

public interface IMetaEssence
{
    Dictionary<string, Sprite> visualEnemy { set; get; }
    string Name { set; get; }
    TypeGroup TypeG { set; get; }
}
