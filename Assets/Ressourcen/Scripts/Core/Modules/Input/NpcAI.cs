using System;
using System.Collections;
using UnityEngine;

public class NpcAI : MonoBehaviour, IInput
{
    public event Action<Vector2> Move;
    public event Action<Vector2> Viwe;
    public event Action<float> CastBolt;

    public event Action<bool> Shoot;
    public event Action Reload;
    public event Action<int> SwitchWeapon;
    public event Action SwitchLight;

    [Header("-----------  AI  ------------")]
    [SerializeField] private bool IsFree = false;
    [SerializeField] private float speedFreeMove;
    [SerializeField] private Vector2 freeArea;
    [SerializeField] private LayerMask layer;
    [SerializeField] private LayerMask layerObject;
    [SerializeField] private float sizeCheckMove;
    [SerializeField] private float sizeCheckAttack;
    [SerializeField] private float sizeCheckObject;
    [SerializeField] private bool AutoReload = false;
    [SerializeField] private WeaponManager weapon;
    [SerializeField] private Transform handR;

    private TypeGroup _group;
    private float _timer = 1;
    private Coroutine _freeMove;
    private Coroutine _read;

    private RaycastHit2D[] _listMove;
    private RaycastHit2D[] _listAttack;
    private RaycastHit2D _listJump;


    public void Initialization(TypeGroup _group)
    {
        this._group = _group;
        if (IsFree)
            _read = StartCoroutine(ReadCollision());
    }

    private void Update()
    {
        if (_timer >= 0)
            _timer -= Time.deltaTime;
    }


    private IEnumerator RandomMove()
    {
        Vector3 _target = new Vector3(UnityEngine.Random.Range(freeArea.x, freeArea.y), 0);
        Vector2 _vec = _target - transform.position;
        
        while (!IsTargetValue(transform.position, _target, 1f))
        {
            _listJump = Physics2D.BoxCast(transform.position, new Vector2(sizeCheckObject, 2), 0f, Vector2.zero, 0f, layerObject);
            Viwe?.Invoke(CalculateDirection(_target));
            Move?.Invoke(new Vector2(
                        speedFreeMove * _vec.normalized.x,
                        _listJump ? 1 : 0));

            yield return new WaitForSeconds(0.1f);
        }

        Move?.Invoke(Vector2.zero);
        _timer = UnityEngine.Random.Range(5, 10);
        _freeMove = null;
    }

    private IEnumerator ReadCollision()
    {
        while (true)
        {
            _listMove = Physics2D.BoxCastAll(transform.position, new Vector2(sizeCheckMove, 2f), 0f, Vector2.zero, 0f, layer);
            yield return new WaitForEndOfFrame();     

            if (FindEnemy(_listMove) != null && !FindEnemy(_listMove).IsDide)
            {
                if (_freeMove != null)
                {
                    StopCoroutine(_freeMove);
                    _freeMove = null;
                }
                
                _listAttack = Physics2D.BoxCastAll(transform.position, new Vector2(sizeCheckAttack, 2f), 0f, Vector2.zero, 0f, layer);
                yield return new WaitForEndOfFrame();

                Viwe?.Invoke(CalculateDirection(FindEnemy(_listMove).transform.position));

                if (FindEnemy(_listAttack) && !FindEnemy(_listAttack).IsDide)
                {
                    if (AutoReload && weapon.CheckAbliveReload() == 0)
                    {
                        Shoot?.Invoke(false);
                        Reload?.Invoke();
                        _timer = UnityEngine.Random.Range(3, 10);
                    }
                    else
                    {
                        if (_timer <= 0)
                        {
                            Shoot?.Invoke(true);
                            _timer = UnityEngine.Random.Range(0.5f, 2);
                        }
                        else
                            Shoot?.Invoke(false);
                    }
                }
                else
                {
                    Shoot?.Invoke(false);
                    if (Move != null)
                    {
                        _listJump = Physics2D.BoxCast(transform.position, new Vector2(sizeCheckObject, 2), 0f, Vector2.zero, 0f, layerObject);
                        Move?.Invoke(new Vector2(
                            (FindEnemy(_listMove).transform.position - transform.position).normalized.x,
                            _listJump ? 1 : 0));
                    }  
                }
            }
            else
            {
                Shoot?.Invoke(false);
                if (_timer <= 0 && _freeMove == null && Move != null)
                    _freeMove = StartCoroutine(RandomMove());
            }
        }
    }

    private CoreObject FindEnemy(RaycastHit2D[] _hits)
    {
        foreach (RaycastHit2D _hit in _hits)
        {
            CoreObject _core;
            if (_core = _hit.collider.gameObject.GetComponentInParent<CoreObject>())
            {
                TypeGroup _t = _core.Group;
                
                if (_t != _group)
                {
                    if (!((_group == TypeGroup.stalker || _group == TypeGroup.clearSky || _t == TypeGroup.scientist) &&
                        (_t == TypeGroup.stalker || _t == TypeGroup.clearSky || _t == TypeGroup.scientist)))
                        return _core;
                }
            }
        }

        return null;
    }

    private Vector2 CalculateDirection(Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - (Vector2)handR.position;
        direction.Normalize();
        return direction;
    }

    private bool IsTargetValue(Vector2 current, Vector2 target, float range = 0.1f)
    {
        Vector2 v = current - target;

        if (Math.Abs(v.x) <= range)
            return true;

        return false;
    }
}
