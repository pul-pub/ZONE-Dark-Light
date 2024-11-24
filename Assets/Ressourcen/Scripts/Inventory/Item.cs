using UnityEngine;

[CreateAssetMenu(menuName = "Item", fileName = "NULL")]
public class Item : ScriptableObject
{
    public int id;
    public string Name;
    public int price = 0;
    public int countCell = 1;
    public int maxCount= 32;
    [Space]
    public Sprite img;
}
