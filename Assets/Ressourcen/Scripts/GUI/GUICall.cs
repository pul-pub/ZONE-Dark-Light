using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GUICall : MonoBehaviour
{
    [SerializeField] private RectTransform objDialog;
    [Space]
    [SerializeField] private TextMeshProUGUI textDialog;

    private float _timer = 0f;
    private Coroutine _call;

    public void OpenDialog(DialogCall _dialog) => 
        _call ??= StartCoroutine(Call(objDialog, new Vector3(0, 350, 0), 5f, textDialog, _dialog));

    private IEnumerator Call(RectTransform _transform, Vector3 _target, float _speed, TextMeshProUGUI _obgText, DialogCall _dialog)
    {
        Vector3 _startPos = _transform.localPosition;
        _transform.gameObject.SetActive(true);

        _obgText.text = _dialog.Dialog;
        DialogCall _now = _dialog;

        while (IsTargetValue(_transform.localPosition, _target, true))
        {
            _transform.localPosition = Vector3.Lerp(_transform.localPosition, _target, _speed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        while (_now)
        {
            _obgText.text = _now.Dialog;
            _timer += _now.TimeVivse;

            while (_timer >= 0f)
            {
                _timer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _now = _now.NextDialog;
        }

        while (IsTargetValue(_transform.localPosition, _startPos, true))
        {
            _transform.localPosition = Vector3.Lerp(_transform.localPosition, _startPos, _speed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }

        _transform.gameObject.SetActive(false);
        _call = null;
    }

    private bool IsTargetValue(Vector3 current, Vector3 target, bool invers = false, float range = 0.1f)
    {
        Vector3 _v = current - target;

        if (Math.Abs(_v.x) <= range &&
            Math.Abs(_v.y) <= range &&
            Math.Abs(_v.z) <= range)
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
