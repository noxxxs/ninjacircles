using UnityEngine;

public class OnShurikenCollideWith : MonoBehaviour
{
    public Transform OwnerTransform;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform != OwnerTransform)
        {
            // Destroy collided object if its Circle
            if (collision.CompareTag("Circle"))
            {
                LevelManager.instance.AllCircles.Remove(collision.transform);
                collision.transform.GetComponent<NinjaCircleMovement>().OnDieCircle();
                Destroy(collision.gameObject);
            }
          
            // Destroy Shuriken if collide with smth
            if (collision.gameObject.layer == 0 && collision.gameObject.GetComponent<OnShurikenCollideWith>() == null && !collision.CompareTag("ShurikenWeapon"))
            {
                Destroy(gameObject);
            }
            
        }
    }
}
