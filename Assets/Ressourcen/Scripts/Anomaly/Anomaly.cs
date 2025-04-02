using System.Collections;
using UnityEngine;

public class Anomaly : MonoBehaviour, IMetaEssence
{
    public string Name { get => nameEssence; }
    public TypeGroup Group { get => group; }
    public bool IsDide { get => false; }
    public ViewEssence Visual
    {
        get
        {
            ViewEssence viw = new ViewEssence();

            viw.Body = spRender.sprite;

            return viw;
        }
    }

    [Header("-----------  Base  -----------")]
    [SerializeField] private string nameEssence;
    [SerializeField] private TypeGroup group;
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
                    Instantiate(objArtifact);
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
