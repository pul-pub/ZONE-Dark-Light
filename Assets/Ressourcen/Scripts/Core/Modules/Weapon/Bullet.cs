using TreeEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet : MonoBehaviour
{
    public float force;
    public float dm;
    public int Layer;

    public IMetaEssence meta;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private SortingLayer layerMask1;
    [SerializeField] private SortingLayer layerMask2;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float maxDistanc = 15f;
    [SerializeField] private Vector2 sizeRay;
    [SerializeField] private Object objHit;

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
        RaycastHit2D[] _hits = Physics2D.BoxCastAll(transform.position, sizeRay, 0f, Vector2.zero, 0f, layerMask);
        if (_hits.Length > 0)
        {
            foreach (RaycastHit2D _hit in _hits)
            {
                CoreObject _core;
                if (_core = _hit.collider.gameObject.GetComponentInParent<CoreObject>())
                {
                    TypeGroup _t = _core.Group;
                    
                    if (_t != meta.Group)
                    {
                        if (!((meta.Group == TypeGroup.stalker || meta.Group == TypeGroup.clearSky || meta.Group == TypeGroup.scientist) &&
                            (_t == TypeGroup.stalker || _t == TypeGroup.clearSky || _t == TypeGroup.scientist)))
                            TakeDamage(_hit);
                    }
                }
                else
                {
                    Instantiate(objHit, transform.position, transform.rotation, transform.parent);
                    Destroy(gameObject);
                }
            }
        }
        transform.Translate(Vector2.right * force * Time.deltaTime);

        if (transform.position.x - _stratPosition.x < maxDistanc * -1 || transform.position.x - _stratPosition.x > maxDistanc)
            Destroy(gameObject);
    }

    private void TakeDamage(RaycastHit2D _hit)
    {
        BodyParthColider _parth = _hit.collider.gameObject.GetComponent<BodyParthColider>();

        if (_parth && _parth.Layer == Layer)
            _parth.ApplyDamage(dm, meta);

        Instantiate(objHit, transform.position, transform.rotation, transform.parent);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, sizeRay);
    }
}
