using MessagePack;

[MessagePackObject]
public class Save
{
    [Key(0)]
    public int FPSMode = 75;
    [Key(1)]
    public float alfaUi = 75;
    [Key(2)]
    public bool vSync = false;
    [Key(3)]
    public bool travsAnim = true;
    [Key(4)]
    public float volSound = 75;
    [Key(5)]
    public float volMusic = 75;
}
