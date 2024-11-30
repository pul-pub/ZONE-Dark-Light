using UnityEngine;

public class EventAnimationHands : MonoBehaviour
{
    [SerializeField] private WeaponManager manager;
    [Header("Stor")]
    [SerializeField] private SpriteRenderer spRender;
    [SerializeField] private Transform pointStor;
    [SerializeField] private Object objStor;
    [SerializeField] private Transform parent;

    public void OnEndErload() 
    {
        manager.EndReload();
        GameObject _gObj = Instantiate(objStor, pointStor.position, pointStor.rotation, parent) as GameObject;

        _gObj.GetComponent<SpriteRenderer>().sprite = spRender.sprite;
    } 
}
