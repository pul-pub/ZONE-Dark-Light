using UnityEngine;

public class BoltManager : MonoBehaviour
{
    [SerializeField] private Transform point;
    [SerializeField] private Object obj;

    private float _force = 0;

    public void OnStartCastelBolt(float _force) => this._force = _force;

    public void OnCastelBolt()
    {
        GameObject _gObj = Instantiate(obj, point.position, Quaternion.Euler(0, 0, transform.localScale.x > 0 ? 20 : -20)) as GameObject;
        
        _gObj.transform.localScale = new Vector3(transform.localScale.x > 0 ? 1 : -1, 1, 1);
        _gObj.GetComponent<Rigidbody2D>().AddForce(
            (transform.localScale.x > 0 ? _gObj.transform.right : -_gObj.transform.right) * (_force * 6),
            ForceMode2D.Impulse);
    }
}
