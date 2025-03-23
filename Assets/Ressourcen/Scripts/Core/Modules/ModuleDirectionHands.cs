using UnityEngine;

public class ModuleDirectionHands : MonoBehaviour
{
    [Header("—————----—  Grafics  ————----——")]
    [SerializeField] private Transform head;
    [SerializeField] private Transform handR;
    [SerializeField] private Transform handL;
    [SerializeField] private Animator handAnim;
    [Space]
    [SerializeField] private bool toRight = true;
    [Header("—————----—  Targets  ————----——")]
    [SerializeField] private Transform targetHandL;
    [Header("—————----—  Modules  ————----——")]
    [SerializeField] private WeaponManager weapon;

    public void OnSetDirection(Vector2 _direction)
    {
        if (head && handR && handL && handAnim)
        {
            if (_direction != Vector2.zero)
            {
                _direction.Normalize();
                float angle = Mathf.Atan2(_direction.y, Mathf.Abs(_direction.x)) * Mathf.Rad2Deg;

                if (weapon.FlagWeapon && !weapon.IsReload)
                {
                    handAnim.enabled = false;

                    if (angle > -40 && angle < 40)
                    {
                        handR.localRotation = Quaternion.Euler(0, 0, angle + 90);

                        Vector2 directionL = targetHandL.position - handL.position;
                        directionL.Normalize();
                        angle = Mathf.Atan2(directionL.y, Mathf.Abs(directionL.x)) * Mathf.Rad2Deg;

                        handL.localRotation = Quaternion.Euler(0, 0, angle + 90);
                    }
                }
                else if (weapon.IsReload)
                    handAnim.enabled = true;

                if (head)
                    head.localRotation = Quaternion.Euler(0, 0, angle / 8);
            }
            else
            {
                if (head)
                    head.localRotation = Quaternion.Euler(0, 0, 0);
                handAnim.enabled = true;
            }
        }

        if ((!toRight && _direction.x > 0) || (toRight && _direction.x < 0))
            Flip();
    }

    private void Flip()
    {
        toRight = !toRight;
        Vector3 scaler = transform.localScale;
        scaler.x = scaler.x * -1;
        transform.localScale = scaler;
    }

    public void Save() => SaveHeandler.SessionNow.pos.flipX = (toRight ? 1 : -1);
    public void Load() => toRight = SaveHeandler.SessionNow.pos.flipX > 0;
}
