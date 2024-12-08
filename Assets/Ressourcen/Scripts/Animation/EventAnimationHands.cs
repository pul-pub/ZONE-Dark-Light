using UnityEngine;

public class EventAnimationHands : MonoBehaviour
{
    [SerializeField] private WeaponManager manager;
    [Header("Stor")]
    [SerializeField] private SpriteRenderer spRender;
    [SerializeField] private Transform pointStor;
    [SerializeField] private Object objStor;
    [SerializeField] private Transform parent;
    [Header("Bolt")]
    [SerializeField] private Transform pointBoltStart;
    [SerializeField] private Object objBolt;

    public void OnEndErload() 
    {
        manager.EndReload();
        GameObject _gObj = Instantiate(objStor, pointStor.position, pointStor.rotation, parent) as GameObject;

        _gObj.GetComponent<SpriteRenderer>().sprite = spRender.sprite;
    }

    public void SpawnBolt()
    {
        GameObject _gObj = Instantiate(
            objBolt, 
            pointBoltStart.position,
            Quaternion.Euler(0, 0, manager.transform.parent.localScale.x > 0 ? 20 : -20),
            parent) as GameObject;

        _gObj.transform.localScale = new Vector3(manager.transform.parent.localScale.x > 0 ? 1 : -1, 1, 1);
        _gObj.GetComponent<Rigidbody2D>().AddForce(
            (manager.transform.parent.localScale.x > 0 ? _gObj.transform.right : -_gObj.transform.right) * manager.ForceBolt,
            ForceMode2D.Impulse);
    }
}
