using UnityEngine;

public class NoRaycastCircle : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private Vector2 _moveDirection;
    [SerializeField] private float _speed;
    Vector3 lastVelocity;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.linearVelocity = _moveDirection * _speed;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        lastVelocity = _rb.linearVelocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        _rb.linearVelocity = direction * Mathf.Max(speed, 0f);
    }

}
