using System;
using UnityEngine;

public class EventAnimationHands : MonoBehaviour
{
    public event Action TakeBolt;
    public event Action EndReload;

    public void OnEndErload() => EndReload?.Invoke();
    public void SpawnBolt() => TakeBolt?.Invoke();
}
