using UnityEngine;

public enum TypeCell { MAIN, SPECIAL };

public class ObjectCell : MonoBehaviour
{
    public int cellID;
    public TypeCell cellType;

    public Vector3 position;
}
