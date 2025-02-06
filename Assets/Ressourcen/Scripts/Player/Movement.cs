using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed = 5f;
    public float ForceJamp = 5f;

    public bool toRight = true;

    public float debufSpeed = 0f;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D col;
    [SerializeField] private Energy energy;

    private Rigidbody2D _rb;
    private float _LocalDebufSpeed = 0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        animator.speed = 1f;
    }

    public void Move(Vector2 _vec)
    {
        if (_vec.x == 0)
        {
            animator.SetBool("IsGo", false);
            animator.SetBool("IsSquat", false);
            col.offset = Vector2.zero;
            animator.speed = 1;
        }
        else
        {
            if (_vec.y <= -0.85f)
            {
                animator.SetBool("IsGo", false);
                if (!animator.GetBool("IsSquat"))
                    animator.SetBool("IsSquat", true);

                col.offset = new Vector2(0, 0.25f); //0.125
                animator.speed = Math.Abs(_vec.x) * 1.5f;
            }
            else if (_vec.y <= 0.7f)
            {
                animator.SetBool("IsSquat", false);
                if (!animator.GetBool("IsGo") && !animator.GetBool("IsSquat"))
                    animator.SetBool("IsGo", true);

                col.offset = Vector2.zero;
                animator.speed = Math.Abs(_vec.x);
            }
            else
            {
                if (Jamp())
                {
                    animator.SetTrigger("Jump");
                    col.offset = Vector2.zero;
                    animator.speed = 1;
                }
            }
        }

        if (energy != null)
        {
            if (_vec.x != 0)
                energy.SetDownEnergy(Math.Abs(_vec.x) / 100);
            else
                energy.SetUpEnergy();

            _LocalDebufSpeed = energy.energy <= 10 ? 3 : 0;
        }

        _rb.linearVelocityX = (_vec.x != 0 ? _vec.x * (Speed - debufSpeed - _LocalDebufSpeed) : 0);

        if (!toRight && _vec.x > 0)
            Flip();
        else if (toRight && _vec.x < 0)
            Flip();
    }

    public void Dide(Vector3 _dideAngel)
    {
        Move(Vector2.zero);
        transform.eulerAngles = _dideAngel;
        col.offset = new Vector2(0, -1f);
    }

    private bool Jamp()
    {
        bool isGrounded = Physics2D.BoxCast(transform.position, new Vector3(1, 2.8f, 0), 0f, new Vector3(), 0f, layerMask);
        if (isGrounded)
        {
            _rb.linearVelocityY = ForceJamp;
            return true;   
        }
        
        return false;
    }

    private void Flip()
    {
        toRight = !toRight;
        Vector3 scaler = transform.localScale;
        scaler.x = scaler.x * -1;
        transform.localScale = scaler;
    }
}
