using UnityEngine;

public class PointAnimation
{
    public Transform tr;
    public Vector2 pos;
    public SpriteRenderer spriteRenderer;

    public Sprite sprite;
    public Vector2 position;
    public Vector3 rotation;

    public float speed;
    public float accuracy;

    public PointAnimation(Transform _tr, Vector3 _pos, SpriteRenderer _spriteRenderer, Sprite _sprite = null, Vector2 _position = default,
                          Vector3 _rotation = default, float _speed = 3f, float _accuracy = 0.01f)
    {
        this.sprite = _sprite;
        this.position = _position;
        this.pos = _pos;
        this.rotation = _rotation;
        this.speed = _speed;
        this.accuracy = _accuracy;

        this.tr = _tr;
        this.spriteRenderer = _spriteRenderer;
    }
}
