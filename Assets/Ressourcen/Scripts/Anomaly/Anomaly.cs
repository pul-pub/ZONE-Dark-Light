using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly : MonoBehaviour, IMetaEnemy
{
    [SerializeField] private string NameAnomaly;
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
    [Header("Artifacts")]
    [SerializeField, Range(0.1f, 100)] private float chanceGiveArtifact;
    [SerializeField] private Object objArtifact;
    [SerializeField] private Transform parent;

    private float _timer = 0f;
    private RaycastHit2D _hit;

    public Dictionary<string, Sprite> visualEnemy { get; set; } = new Dictionary<string, Sprite>();
    public string Name { get; set; }


    private void Awake()
    {
        Name = NameAnomaly;
        visualEnemy.Add("Anomaly", spRender.sprite);
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
                    _health.ApplyDamage(dm, this);

                animator.SetTrigger("Attack");
                _timer = startTimeBtwShot;

                int _chackChance = Random.Range(0, 100);

                if (_chackChance >= chanceGiveArtifact)
                {
                    GameObject _gObj = Instantiate(objArtifact, parent) as GameObject;
                }
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
