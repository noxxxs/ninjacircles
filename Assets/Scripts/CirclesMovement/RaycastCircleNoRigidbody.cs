using UnityEngine;


public class RaycastCircleNoRigidbody : MonoBehaviour
{
    public float speed = 20f;
    public float rayLength = 1f;
    public int maxRicochets = 5;

    [SerializeField] private Vector2 direction;
    private int ricochetCount = 0;

    void Update()
    {
        Vector2 currentPosition = transform.position;
        Vector2 nextPosition = currentPosition + direction * speed * Time.deltaTime;

        // Виконуємо Raycast на шляху руху
        RaycastHit2D hit = Physics2D.Raycast(currentPosition, direction, speed * Time.deltaTime);

        if (hit.collider != null)
        {
            // Відображення напрямку відносно нормалі зіткнення
            direction = Vector2.Reflect(direction, hit.normal).normalized;
            ricochetCount++;

            if (ricochetCount > maxRicochets)
            {
                Destroy(gameObject);
                return;
            }

            // Переміщуємо кулю в точку зіткнення плюс невеликий відступ
            transform.position = hit.point + hit.normal * 0.01f;
        }
        else
        {
            // Немає зіткнення — просто рухаємося вперед
            transform.position = nextPosition;
        }

        // Повертаємо кулю у напрямку руху (необов’язково)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
