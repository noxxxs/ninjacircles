using UnityEngine;

public class MovementScreenEdgeLogic : MonoBehaviour
{
    public Vector2 Direction;
    public float speed;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.linearVelocity = Direction.normalized * speed;
    }
}
