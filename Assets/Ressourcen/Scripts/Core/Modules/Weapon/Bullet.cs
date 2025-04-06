using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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
    [SerializeField] private Object objHitDamage;

    private Vector2 _stratPosition;
    private List<Collision2D> _ex = new();

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
        //List<RaycastHit2D> _hits = Physics2D.RaycastAll(transform.position, transform.position + transform.right, 0.1f, layerMask).ToList();
        //BodyParthColider _parth = null;
        //TypeGroup _t = TypeGroup.millitary;
        //if (_hits.Find(h => (_parth = h.collider.gameObject.GetComponent<BodyParthColider>()) &&
        //    (_t = _parth.gameObject.GetComponentInParent<CoreObject>().Group) != meta.Group &&
        //    !((meta.Group == TypeGroup.stalker || meta.Group == TypeGroup.clearSky || meta.Group == TypeGroup.scientist) &&
        //    (_t == TypeGroup.stalker || _t == TypeGroup.clearSky || _t == TypeGroup.scientist))))
        //{
        //    _parth.ApplyDamage(dm, meta);
        //    Instantiate(objHitDamage, transform.position, transform.rotation, transform.parent);
        //    Destroy(gameObject);
        //}

        transform.Translate(Vector2.right * force * Time.deltaTime);

        if (transform.position.x - _stratPosition.x < maxDistanc * -1 || transform.position.x - _stratPosition.x > maxDistanc)
            Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CoreObject _core;
        BodyParthColider _parth;
        TypeGroup _t;

        if (_parth = collision.collider.gameObject.GetComponent<BodyParthColider>())
        {
            if (_parth.Layer == Layer)
                return;

            if (_core = collision.collider.gameObject.GetComponentInParent<CoreObject>())
            {
                if ((_t = _core.Group) != meta.Group &&
                    !((meta.Group == TypeGroup.stalker || meta.Group == TypeGroup.clearSky || meta.Group == TypeGroup.scientist) &&
                    (_t == TypeGroup.stalker || _t == TypeGroup.clearSky || _t == TypeGroup.scientist)))
                {
                    _parth.ApplyDamage(dm, meta);
                    Instantiate(objHitDamage, transform.position, transform.rotation, transform.parent);
                }
                else
                    return;
            }
        }
        else
            Instantiate(objHit, transform.position, transform.rotation, transform.parent);

        Destroy(gameObject);
    }
}
