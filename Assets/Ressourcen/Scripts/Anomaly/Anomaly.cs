using System.Collections;
using UnityEngine;

public class Anomaly : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spRender;
    [Header("Attack")]
    [SerializeField] private LayerMask layer;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float sizeCheckShoot;
    [Space]
    [SerializeField] private float dm = 10;
    [SerializeField] private float startTimeBtwShot = 3;

    private float _timer = 0f;
    private RaycastHit2D _hit;

    private void Awake()
    {
        StartCoroutine(Check());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + offset, sizeCheckShoot);
    }

    public void SetOffVisual() => spRender.enabled = false;

    IEnumerator Check()
    {
        while (true)
        {
            _hit = Physics2D.BoxCast(transform.position, new Vector2(sizeCheckShoot, 2f), 0f, offset.normalized, offset.magnitude, layer);

            yield return new WaitForEndOfFrame();
            
            if (_hit)
            {
                Health _health;
                if (_health = _hit.collider.gameObject.GetComponentInParent<Health>())
                    _health.ApplyDamage(dm);

                animator.SetTrigger("Attack");
                _timer = startTimeBtwShot;
            }

            while (_timer >= 0f)
            {
                _timer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            spRender.enabled = true;
        }
    }
}
