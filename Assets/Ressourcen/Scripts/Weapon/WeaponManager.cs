using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private GUIHandler handlerGUI;

    private float _timer = 0;

    public bool IsShoot { private set; get; } = false;

    private void Update()
    {
        if (_timer >= 0)
            _timer -= Time.deltaTime;

        if (IsShoot)
            Shoot();
    }

    private void Shoot()
    {
        if (_timer <= 0)
        {
            Debug.Log("f");
            _timer = 0.1f;
        }
    }

    public void SetIsShoot(bool _isShoot) => IsShoot = _isShoot;
}
