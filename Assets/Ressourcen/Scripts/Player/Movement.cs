using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed = 5f;
    public float ForceJamp = 5f;

    public bool toRight = true;
    public bool isSit = false;

    [SerializeField] private LayerMask _layerMask;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 _vec)
    {
        if (!isSit)
            _rb.linearVelocityX = _vec.x * Speed;

        if (_vec.y > 0.7f)
            Jamp();

        if (!toRight && _vec.x > 0)
            Flip();
        else if (toRight && _vec.x < 0)
            Flip();
    }

    public void Jamp()
    {
        bool isGrounded = Physics2D.BoxCast(transform.position, new Vector3(1, 3, 0), 0f, new Vector3(), 0f, _layerMask);
        if (isGrounded)
            _rb.linearVelocityY = ForceJamp;
    }

    private void Flip()
    {
        toRight = !toRight;
        Vector3 scaler = transform.localScale;
        scaler.x = scaler.x * -1;
        transform.localScale = scaler;
    }
}
