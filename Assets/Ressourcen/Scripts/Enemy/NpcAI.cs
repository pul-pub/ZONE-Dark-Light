using System;
using System.Collections;
using UnityEngine;

public class NpcAI : MonoBehaviour
{
    public event Action<Vector2> OnMove;
    public event Action OnAttack;

    [Header("AI")]
    public bool IsFreePerson = false;
    public Vector2 freeArea;
    [Space]
    public LayerMask Layer;
    public LayerMask LayerObject;
    public float SizeCheckMove;
    public float SizeCheckAttack;
    public float SizeCheckObject;

    private float _timer = 1;
    private Coroutine _freeMove;
    private Coroutine _read;

    private RaycastHit2D _listMove;
    private RaycastHit2D _listAttack;
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
        
        while (IsTargetValue(transform.position, _target, true))
        {
            OnMove.Invoke(new Vector2(
                        0.25f * _vec.normalized.x,
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
            _listMove = Physics2D.BoxCast(transform.position, new Vector2(SizeCheckMove, 0.25f), 0f, Vector2.zero, 0f, Layer);
            yield return new WaitForEndOfFrame();
            _listJump = Physics2D.BoxCast(transform.position, new Vector2(SizeCheckObject, 2), 0f, Vector2.zero, 0f, LayerObject);
            yield return new WaitForEndOfFrame();
            _listAttack = Physics2D.BoxCast(transform.position, new Vector2(SizeCheckAttack, 0.25f), 0f, Vector2.zero, 0f, Layer);

            if (_listMove || _listAttack)
            {
                if (_freeMove != null)
                {
                    StopCoroutine(_freeMove);
                    _freeMove = null;
                }

                if (_listAttack)
                {
                    if (OnAttack != null)
                        OnAttack.Invoke();

                    if (IsFreePerson && OnMove != null)
                        OnMove.Invoke(new Vector2((_listMove.collider.transform.position - transform.position).x > 0 ? 0.0001f: -0.0001f, 0));
                }
                else
                    if (IsFreePerson && OnMove != null)
                    OnMove.Invoke(new Vector2(
                        (_listMove.collider.transform.position - transform.position).normalized.x,
                        _listJump ? 1 : 0));
            }
            else
            {
                if (IsFreePerson && _timer <= 0 && _freeMove == null && OnMove != null)
                    _freeMove = StartCoroutine(RandomMove());
            }
        }
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
