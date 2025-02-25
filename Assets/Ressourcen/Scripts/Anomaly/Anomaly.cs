using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anomaly : MonoBehaviour, IMetaEssence
{
    [SerializeField] private string NameAnomaly;
    [SerializeField] private TypeGroup TypeBaseG;
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
    private RaycastHit2D[] _hit;

    public Dictionary<string, Sprite> visualEnemy { get; set; } = new Dictionary<string, Sprite>();
    public string Name { get; set; }
    public TypeGroup TypeG { get; set; }

    private void Awake()
    {
        TypeG = TypeBaseG;
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
            _hit = Physics2D.BoxCastAll(transform.position, new Vector2(sizeCheckShoot, 4f), 0f, offset.normalized, offset.magnitude, layer);

            yield return new WaitForEndOfFrame();
            
            if (_hit.Length > 0)
            {
                animator.SetTrigger("Attack");
                _timer = startTimeBtwShot;

                yield return new WaitForSeconds(0.2f);

                _hit = Physics2D.BoxCastAll(transform.position, new Vector2(sizeCheckShoot, 4f), 0f, offset.normalized, offset.magnitude, layer);

                foreach (RaycastHit2D _h in _hit)
                {
                    BodyParthColider _parth;
                    if (_parth = _h.collider.GetComponent<BodyParthColider>())
                        _parth.ApplyDamage(dm, this);
                }

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
