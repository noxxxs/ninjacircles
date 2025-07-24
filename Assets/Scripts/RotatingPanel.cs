using UnityEngine;

public class RotatingPanel : MonoBehaviour
{
    public float rotationSpeed = 90f;

    void FixedUpdate()
    {
        transform.Rotate(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
    }
}
