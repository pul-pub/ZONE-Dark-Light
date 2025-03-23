using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private Vector2 sizeCheck;
    [Space]
    [SerializeField] private List<SpriteRenderer> spRender;

    private Dictionary<SpriteRenderer, Coroutine> _anims = new();

    private void Awake()
    {
        StartCoroutine(Check());
    }

    private IEnumerator Check()
    {
        while (true)
        {
            RaycastHit2D _hit = Physics2D.BoxCast(transform.position, sizeCheck, 0f, Vector2.zero, 0f, layer);

            foreach (SpriteRenderer _render in spRender)
            {
                if (_render.color.a == (_hit ? 1 : 0))
                {
                    if (!_anims.TryAdd(_render, null))
                        if (_anims[_render] != null)
                            StopCoroutine(_anims[_render]);

                    _anims[_render] = StartCoroutine(SetAlfa(_render, (short)(_hit ? -4 : 4)));
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator SetAlfa(SpriteRenderer _sr, short _factor = 4)
    {
        _sr.gameObject.SetActive(true);

        short a = (short)(_factor > 0 ? 0 : 255);
        while ((_factor > 0 && _sr.color.a < 0.95) || (_factor < 0 && _sr.color.a > 0.05))
        {
            a += _factor;
            _sr.color = new Color32(255, 255, 255, (byte)a);
            yield return new WaitForEndOfFrame();
        }

        if (_factor < 0)
        {
            _sr.gameObject.SetActive(false);
            _sr.color = new Color32(255, 255, 255, 0);
        }
        else
            _sr.color = new Color32(255, 255, 255, 255);
    }
}
