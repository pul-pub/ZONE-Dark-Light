using System;
using System.Collections;
using UnityEngine;

public class AnimatorObject
{
    public event Action<int> OnEnd;

    public IEnumerator MoveAnimation(PointAnimation[] _animation)
    {
        for (int i = 0; i < _animation.Length; i++)
        {
            if (_animation[i].position != default)
            {
                if (_animation[i].tr == null)
                {
                    while (IsTargetValue(_animation[i].tr.position, _animation[i].position, true, _animation[i].accuracy))
                    {
                        _animation[i].tr.position = Vector2.MoveTowards(_animation[i].tr.position, _animation[i].position, _animation[i].speed * Time.deltaTime);

                        yield return new WaitForEndOfFrame();
                    }
                }
                else
                {
                    while (IsTargetValue(_animation[i].tr.position, _animation[i].position, true, _animation[i].accuracy))
                    {
                        _animation[i].tr.position = Vector2.MoveTowards(_animation[i].tr.position, _animation[i].position, _animation[i].speed * Time.deltaTime);

                        yield return new WaitForEndOfFrame();
                    }
                }
            }
        }
    }

    public IEnumerator RotationAnimation(PointAnimation[] _animation, int _id)
    {
        for (int i = 0; i < _animation.Length; i++)
        {
            while (IsTargetValue(_animation[i].tr.localEulerAngles, _animation[i].rotation, true))
            {
                //_animation[i].tr.position = Vector2.MoveTowards(_animation[i].tr.position, _animation[i].position, _animation[i].speed * Time.deltaTime);
                _animation[i].tr.localEulerAngles = _animation[i].rotation.normalized * (_animation[i].speed * Time.deltaTime);
                Debug.Log(_animation[i].tr.localEulerAngles);
                yield return new WaitForEndOfFrame();
            }
        }

        if (OnEnd != null)
            OnEnd.Invoke(_id);
    }

    public bool IsTargetValue(Vector2 current, Vector2 target, bool invers = false, float range = 0.1f)
    {
        Vector2 v = current - target;

        if (v.x <= range && v.x >= -range &&
            v.y <= range && v.y >= -range)
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
