using System;
using System.Collections;
using UnityEngine;

public class NpcAI : MonoBehaviour
{
    public event Action<Vector2> OnMove;
    public event Action OnAttack;

    [Header("AI")]
    public bool IsFreePerson = false;
    public float FreeSpeed = 0.25f;
    public Vector2 freeArea;
    [Space]
    public LayerMask Layer;
    public LayerMask LayerObject;
    public float SizeCheckMove;
    public float SizeCheckAttack;
    public float SizeCheckObject;
    [Space]
    protected TypeGroup typeGroup;

    private float _timer = 1;
    private Coroutine _freeMove;
    private Coroutine _read;

    private RaycastHit2D[] _listMove;
    private RaycastHit2D[] _listAttack;
    private RaycastHit2D _listJump;

    public void Update()
    {
        if (_timer >= 0)
            _timer -= Time.deltaTime;

        if (_read == null)
            _read = StartCoroutine(ReadCollision());
    }

    private void OnEndCorutline()
    {
        _freeMove = null;
    }

    IEnumerator RandomMove()
    {
        Vector3 _target = new Vector3(UnityEngine.Random.Range(freeArea.x, freeArea.y), 0);
        Vector2 _vec = _target - transform.position;
        
        while (IsTargetValue(transform.position, _target, true, 1f))
        {
            OnMove.Invoke(new Vector2(
                        FreeSpeed * _vec.normalized.x,
                        _listJump ? 1 : 0));

            yield return new WaitForSeconds(0.1f);
        }

        OnMove.Invoke(Vector2.zero);
        _timer = UnityEngine.Random.Range(5, 10);
        OnEndCorutline();
    }

    IEnumerator ReadCollision()
    {
        while (true)
        {
            _listMove = Physics2D.BoxCastAll(transform.position, new Vector2(SizeCheckMove, 2f), 0f, Vector2.zero, 0f, Layer);
            yield return new WaitForEndOfFrame();
            _listJump = Physics2D.BoxCast(transform.position, new Vector2(SizeCheckObject, 2), 0f, Vector2.zero, 0f, LayerObject);
            yield return new WaitForEndOfFrame();
            _listAttack = Physics2D.BoxCastAll(transform.position, new Vector2(SizeCheckAttack, 2f), 0f, Vector2.zero, 0f, Layer);

            if (FindEnemy(_listAttack) != new RaycastHit2D() || FindEnemy(_listMove) != new RaycastHit2D())
            {
                if (_freeMove != null)
                {
                    StopCoroutine(_freeMove);
                    _freeMove = null;
                }

                if (FindEnemy(_listAttack) != new RaycastHit2D())
                {
                    if (FindEnemy(_listAttack) != new RaycastHit2D())
                    {
                        OnAttack?.Invoke();
                        OnMove.Invoke(new Vector2((FindEnemy(_listAttack).collider.transform.position - transform.position).x > 0 ? 0.0001f : -0.0001f, 0));
                    }
                }
                else
                    if (IsFreePerson && OnMove != null)
                        OnMove.Invoke(new Vector2(
                            (FindEnemy(_listMove).collider.transform.position - transform.position).normalized.x,
                            _listJump ? 1 : 0));
            }
            else
            {
                if (IsFreePerson && _timer <= 0 && _freeMove == null && OnMove != null)
                    _freeMove = StartCoroutine(RandomMove());
            }
        }
    }

    private RaycastHit2D FindEnemy(RaycastHit2D[] _hits)
    {
        foreach (RaycastHit2D _hit in _hits)
        {
            if (_hit.collider.gameObject.GetComponentInParent<Player>() && 
                (typeGroup == TypeGroup.Millitary || typeGroup == TypeGroup.Bandut || typeGroup == TypeGroup.Mutant))
                return _hit;
            else
            {
                if (_hit.collider.gameObject.GetComponentInParent<NPC>().backpack == null)
                {
                    TypeGroup _t = _hit.collider.gameObject.GetComponentInParent<NPC>().TypeG;
                    if (_t != typeGroup)
                    {
                        if ((typeGroup == TypeGroup.Stalker || typeGroup == TypeGroup.ClearSky) &&
                            (_t == TypeGroup.Stalker || _t == TypeGroup.ClearSky))
                            Debug.Log("1");
                        else
                            return _hit;
                    }
                }
            }
        }

        return new();
    }

    public bool IsTargetValue(Vector2 current, Vector2 target, bool invers = false, float range = 0.1f)
    {
        Vector2 v = current - target;

        if (Math.Abs(v.x) <= range)
        {
            if (!invers)
                return true;

            return false;
        }

        if (!invers)
            return false;

        return true;
    }
}
