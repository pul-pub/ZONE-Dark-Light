using System.Collections.Generic;
using UnityEngine;

public interface IMetaEnemy
{
    Dictionary<string, Sprite> visualEnemy { set; get; }
    string Name { set; get; }
}
