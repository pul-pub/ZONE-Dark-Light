using MessagePack;

[MessagePackObject]
public class Save
{
    [Key(0)]
    public float alfaUi;
    [Key(1)]
    public float volSound;
    [Key(2)]
    public bool vibroMode;
    [Key(3)]
    public bool promptMode;
    [Key(4)]
    public int FPSMode;
}
