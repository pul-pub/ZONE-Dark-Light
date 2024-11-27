using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    private AnimatorObject _anim = new AnimatorObject();

    private Vector3 _offset = new Vector3(4, -0.8f, -10);
    private Coroutine _coroutine;

    private void Update()
    {
        if (_offset.x < 0 && target.localScale.x > 0)
        {           
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            _coroutine = StartCoroutine(Move(new Vector3(4, -0.8f, -10)));
        }
        else if (_offset.x > 0 && target.localScale.x < 0)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            _coroutine = StartCoroutine(Move(new Vector3(-4, -0.8f, -10)));
        }

        transform.position = target.position + _offset;
    }

    private IEnumerator Move(Vector3 _to)
    {
        while (_anim.IsTargetValue(_offset, _to, true, 0.01f))
        {
            _offset = Vector3.MoveTowards(_offset, _to, 15 * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
    }
}
