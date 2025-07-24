using Microlight.MicroAudio;
using UnityEngine;

public class ShurikenWeapon : MonoBehaviour
{
    public Transform WeaponOwnerTransform;
    public float CoolDown;
    public int Capacity ;
    public float ShurikenSpeed;
    public LayerMask AttackLayer;
    public float CircleRadius;

    private Transform _target;
    private float _timer;
    void FixedUpdate()
    {
        _target = FindTarget();
        if (_target != null)
        {
            PrepareToAction();
        }
    }

    private Transform FindTarget()
    {
        foreach (Transform circle in LevelManager.instance.AllCircles)
        {
            Vector2 direction = (circle.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, circle.position);

            Vector2 rayOrigin = (Vector2)transform.position + direction * CircleRadius + direction * CircleRadius * 0.5f;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, distance, AttackLayer);

            if (hit.collider != null && hit.collider.transform != WeaponOwnerTransform && hit.collider.CompareTag("Circle"))
            {
                Debug.DrawLine(rayOrigin, hit.point, Color.red, 0.2f);
                return circle;
            }
        }

        return null;
    }

    private void PrepareToAction()
    {
        _timer += Time.deltaTime;

        if (_timer >= CoolDown)
        {
            ThrowShuriken();
            _timer = 0;
            
        }
    }

    private void ThrowShuriken()
    {
        // Spawn shuriken and give him velocity
        GameObject shuriken = Instantiate(LevelManager.instance.ShurikenSO.ThrowableShurikenPrefab, transform.position, Quaternion.identity);
        shuriken.GetComponent<OnShurikenCollideWith>().OwnerTransform = transform;
        shuriken.GetComponent<Rigidbody2D>().linearVelocity = (_target.position - transform.position).normalized * ShurikenSpeed * 10;
        shuriken.GetComponent<Rigidbody2D>().AddTorque(360f);
        MicroAudio.PlayEffectSound(LevelManager.instance.ThrowShurikenGroup.GetRandomClip);

        Capacity--;
        if (Capacity <= 0)
        {
            WeaponOwnerTransform.GetComponent<NinjaCircleMovement>().WaitToTakeNextWeapon(LevelManager.instance.ShurikenSO.CoolDownToTakeAgain);
            DropItemFromHand();
            Destroy(this);
        }
    }

    private void DropItemFromHand()
    {
        Debug.Log("droppedItem");
        GameObject droppedShuriken = Instantiate(LevelManager.instance.ShurikenSO.OnCollideWithShurikenPrefab, transform.position, Quaternion.identity);
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
    }
}
