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

    private Collider2D[] _listMove;
    private Collider2D[] _listAttack;
    private Collider2D[] _listJump;

    public void Update()
    {
        if (_timer >= 0)
            _timer -= Time.deltaTime;

        _listMove = Physics2D.OverlapCircleAll(transform.position, SizeCheckMove, Layer);
        _listAttack = Physics2D.OverlapCircleAll(transform.position, SizeCheckAttack, Layer);
        _listJump = Physics2D.OverlapCircleAll(transform.position, SizeCheckObject, LayerObject);

        if (_listMove.Length > 0 || _listAttack.Length > 0)
        {
            if (_freeMove != null)
            {
                StopCoroutine(_freeMove);
                _freeMove = null;
            }

            if (_listAttack.Length > 0)
            {
                if (OnAttack != null)
                    OnAttack.Invoke();

                if (IsFreePerson && OnMove != null)
                    OnMove.Invoke(Vector2.zero);
            }
            else
                if (IsFreePerson && OnMove != null)
                    OnMove.Invoke(new Vector2(
                        (_listMove[0].transform.position - transform.position).normalized.x,
                        _listJump.Length > 0 ? 1 : 0));
        }
        else
        {
            if (IsFreePerson && _timer <= 0 && _freeMove == null && OnMove != null)
                _freeMove = StartCoroutine(RandomMove());
        }
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
                        0.5f * _vec.normalized.x,
                        _listJump.Length > 0 ? 1 : 0));

            yield return new WaitForEndOfFrame();
        }

        OnMove.Invoke(Vector2.zero);
        _timer = UnityEngine.Random.Range(5, 10);
        OnEndCorutline();
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
