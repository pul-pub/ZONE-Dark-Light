using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed = 5f;
    public float ForceJamp = 5f;

    public bool toRight = true;

    public float _debufSpeed = 0f;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Animator animator;

    private Rigidbody2D _rb;

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
            animator.speed = 1;
        }
        else
        {
            if (_vec.y <= -0.85f)
            {
                animator.SetBool("IsGo", false);
                if (!animator.GetBool("IsSquat"))
                    animator.SetBool("IsSquat", true);

                animator.speed = Math.Abs(_vec.x) * 1.5f;
            }
            else if (_vec.y <= 0.7f)
            {
                animator.SetBool("IsSquat", false);
                if (!animator.GetBool("IsGo") && !animator.GetBool("IsSquat"))
                    animator.SetBool("IsGo", true);

                animator.speed = Math.Abs(_vec.x);
            }
            else
            {
                if (Jamp())
                {
                    animator.SetTrigger("Jump");
                    animator.speed = 1;
                }
            }
        }
            
        _rb.linearVelocityX = (_vec.x != 0 ? _vec.x * Speed - _debufSpeed : 0);
            
        if (!toRight && _vec.x > 0)
            Flip();
        else if (toRight && _vec.x < 0)
            Flip();
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

    private void Animation(string _name, float _speed)
    {
        if (_name == "Jump")
        {
            animator.SetTrigger("Jump");
            animator.speed = 1;
        }
        else if (name == "Go")
        {
            
            Debug.Log("GO ---- " + _speed);
        }
        else if (_name == "Squat")
        {
            animator.SetInteger("Move", 2);
            animator.speed = _speed;
            Debug.Log("SQUAT ---- " + _speed);
        }
        else
        {
            
        }
    }

    private void Flip()
    {
        toRight = !toRight;
        Vector3 scaler = transform.localScale;
        scaler.x = scaler.x * -1;
        transform.localScale = scaler;
    }
}
