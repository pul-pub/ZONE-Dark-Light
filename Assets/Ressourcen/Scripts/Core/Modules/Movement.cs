using UnityEngine;

public class Movement : MonoBehaviour
{
    public float _mass;
    public float _massMax;
    public float _hpLeg;
    public float _hpLegMax;

    [Header("----------  Base  -----------")]
    [SerializeField] private float Speed = 5f;
    [SerializeField] private float ForceJamp = 5f;
    [Header("---------  Modules  ---------")]
    [SerializeField] private Animator animatorLeg;
    [SerializeField] private BoxCollider2D colLeg;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Energy energy;
    [Header("---------  Grafic  ----------")]
    [SerializeField] private LayerMask layerGround;
    [Space]
    [SerializeField] private Object objMoveEffect;
    [SerializeField] private Transform posMoveEffect;

    private float _debuf { get => (energy ? 1 - energy.Value / 100 : 0) + (1 - _mass / _massMax) + (1 - _hpLeg / _hpLegMax); }
    private float _timer = 1f;

    private void Update()
    {
        if (_timer >= 0)
            _timer -= Time.deltaTime;
    }

    public void Move(Vector2 _vec)
    {
        if (System.Math.Abs(_vec.x) > 0.01)
        {
            if (_vec.y > 0.7f && Jamp())
                animatorLeg.SetTrigger("Jump");

            animatorLeg.SetBool("IsGo", _vec.y >= -0.85f && _vec.y <= 0.7f);
            animatorLeg.SetBool("IsSquat", _vec.y < -0.85f);
            colLeg.offset = _vec.y < -0.85f ? new Vector2(0, 0.3f) : Vector2.zero;
            animatorLeg.speed = System.Math.Abs(_vec.x) * (_vec.y < -0.85f ? 1.5f : 1);

            if (_vec.x != 0 && _timer <= 0f && objMoveEffect != null)
            {
                GameObject _gObj = Instantiate(objMoveEffect, posMoveEffect.position, posMoveEffect.rotation, transform.parent) as GameObject;
                _gObj.transform.localPosition += _vec.y > -0.85f ? Vector3.zero : new Vector3(0, 0.12f, 0);
                _gObj.transform.localScale = new Vector3(transform.localScale.x > 0 ? 1.5f : -1.5f, 1.5f, 1.5f);
                _timer = 0.5f;
            }

            if (_vec.x != 0 && energy)
                energy.SetDownEnergy(System.Math.Abs(_vec.x) / 100);

            rb.linearVelocityX = (_vec.x != 0 ? _vec.x * (Speed - _debuf) : 0);
        }
        else
        {
            animatorLeg.SetBool("IsGo", false);
            animatorLeg.SetBool("IsSquat", false);
            animatorLeg.speed = 1;
            //colLeg.offset = Vector2.zero;

            rb.linearVelocityX = 0;
        }
    }

    public void Dide(Vector3 _dideAngel)
    {
        Move(Vector2.zero);
        transform.eulerAngles = _dideAngel;
        colLeg.offset = new Vector2(0, -1f);
    }

    private bool Jamp()
    {
        bool isGrounded = Physics2D.BoxCast(transform.position, new Vector3(1, 2.8f, 0), 0f, new Vector3(), 0f, layerGround);
        if (isGrounded)
            rb.linearVelocityY = ForceJamp;

        return isGrounded;
    }
}
