using System;
using System.Collections;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [SerializeField] private Transform referenceBody;
    [SerializeField] private LayerMask layer;

    private Coroutine _ckecking;

    public void Checking(GUISpinner _amplitude, GUISpinner _period, float _raduius)
    {
        if (_raduius != -1 && _ckecking == null)
            _ckecking = StartCoroutine(Check(_amplitude, _period, _raduius));
        else if (_raduius == -1 && _ckecking != null)
        {
            StopCoroutine(_ckecking);
            _ckecking = null;
        }
    }

    IEnumerator Check(GUISpinner _amplitude, GUISpinner _period, float _raduius)
    {
        while (true) 
        {
            RaycastHit2D _hit = Physics2D.BoxCast(transform.position, new Vector2(_raduius, 3f), 0f, Vector2.zero, 0f, layer);

            yield return new WaitForEndOfFrame();
            
            if (_hit)
            {
                Collider2D _col = _hit.collider;

                Artifact _art;
                
                if (_art = _col.GetComponent<Artifact>())
                {
                    if (!_art.IsFinded)
                    {
                        if ((float)Math.Round(_amplitude.Value, 4, MidpointRounding.ToEven) == _art.ArtifactObject.Amplitude &&
                            (float)Math.Round(_period.Value, 4, MidpointRounding.ToEven) == _art.ArtifactObject.Period)
                        {
                            _art.SetFinded();
                            yield return new WaitForEndOfFrame();
                        }

                        yield return new WaitForEndOfFrame();
                    }
                }
            }
        }
    }
}
