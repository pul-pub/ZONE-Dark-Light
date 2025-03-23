using UnityEngine;

public interface IItem
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Discription { get; set; }

    public int Price { get; set; }
    public int CountCell { get; set; }
    public int MaxCount { get; set; }
    public float Weight { get; set; }
    public float Condition { get; set; }
    public Sprite Img { get; set; }

    public int Value { get; set; }


    public string Use();
    public int Restor(int _value);
    public int Reload(int _value);


    public IItem CloneItem();
}
