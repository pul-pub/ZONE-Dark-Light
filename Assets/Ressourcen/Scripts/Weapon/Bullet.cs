using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet : MonoBehaviour
{
    public float force;
    public float dm;
    public int Layer;

    public IMetaEnemy meta;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private SortingLayer layerMask1;
    [SerializeField] private SortingLayer layerMask2;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float maxDistanc = 15f;
    [SerializeField] private Vector2 sizeRay;

    private Vector2 _stratPosition;

    private void Awake()
    {
        _stratPosition = transform.position;

        if (Layer > 1)
            trailRenderer.sortingLayerID = 4;
        else if (Layer < -1)
            trailRenderer.sortingLayerID = 7;
    } 

    void Update()
    {
        RaycastHit2D[] _hit = Physics2D.BoxCastAll(transform.position, sizeRay, 0f, Vector2.zero, 0f, layerMask);
        if (_hit.Length > 0)
        {
            foreach (RaycastHit2D _h in _hit)
            {
                BodyParthColider _parth = _h.collider.gameObject.GetComponent<BodyParthColider>();

                if (_parth && _parth.Layer == Layer)
                {
                    _parth.ApplyDamage(dm, meta);
                    Destroy(gameObject);
                }
            }
        }
        transform.Translate(Vector2.right * force * Time.deltaTime);

        if (transform.position.x - _stratPosition.x < maxDistanc * -1 || transform.position.x - _stratPosition.x > maxDistanc)
            Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, sizeRay);
    }
}
