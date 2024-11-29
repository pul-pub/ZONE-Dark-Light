using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    public float LifeTime = 5f;

    private float _timer = 0;

    private void Update()
    {
        TimeDestroy(gameObject);
    }

    public void TimeDestroy(GameObject _obj)
    {
        if (_timer <= LifeTime)
            _timer += Time.deltaTime;
        else
            Destroy(_obj);
    }
}
