using Microlight.MicroAudio;
using PrimeTween;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class NinjaCircleMovement : MonoBehaviour
{
    public enum CircleTypeEnum { Red, Yellow, Green, Blue }
    [SerializeField] private CircleTypeEnum _circleType;
    public CircleTypeEnum CircleType
    {
        get { return _circleType; }  
    }
    private Rigidbody2D _rb;
    private CircleCollider2D _collider;
    [SerializeField] private float _speed;
    [SerializeField] private float _volume;
    [SerializeField] private Vector2 _direction;
    [SerializeField] private ParticleSystem _dieParticlesPrefab;
    [SerializeField] MicroSoundGroup _soundGroup;

    private bool _canMove = true;
    private bool _hasWeapon = false;
    public bool HasWeapon
    {
        get { return _hasWeapon; }
        set { _hasWeapon = value; }
    }
    [SerializeField] private LayerMask _attackLayer;
    [SerializeField] private float _shurikenSpeed;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        LevelManager.instance.AllCircles.Add(transform);
        if (_canMove)
        _rb.linearVelocity = _direction.normalized * _speed;

        MicroAudio.MasterVolume = _volume;

        topLeft.x = _topLeftTransform.position.x;
        topLeft.y = _topLeftTransform.position.y;
        bottomRight.x = _bottomRightTransform.position.x;
        bottomRight.y = _bottomRightTransform.position.y;
    }

    private Vector2 _lastVelocity;

    void FixedUpdate()
    {
        if (!_canMove)
            return;

        if (_rb.linearVelocity.magnitude <= 0.1f)
            TryUnstick();

        Vector2 currentVelocity = _rb.linearVelocity;

        // Check for changes
        if (_lastVelocity != Vector2.zero && Vector2.Angle(currentVelocity, _lastVelocity) > 1f)
        {
            //MicroAudio.PlayEffectSound(_soundGroup.GetRandomClip);
            Tween.PunchScale(transform, Vector3.one * 0.3f, 0.2f);
        }

        // Save target speed
        if (currentVelocity.magnitude != _speed)
        {
            _rb.linearVelocity = currentVelocity.normalized * _speed;
        }
        _lastVelocity = _rb.linearVelocity;

       
    }

    private int steps = 32;
    private float initialSearchRadius = 2.5f;
    private int maxIterations = 15;
    private Vector2 topLeft;
    [SerializeField] private Transform _topLeftTransform;
    private Vector2 bottomRight;
    [SerializeField] private Transform _bottomRightTransform;
    private void TryUnstick()
    {
        
        Vector2 origin = transform.position;
        float radius = _collider.radius * transform.lossyScale.x;

        float minX = topLeft.x;
        float maxX = bottomRight.x;
        float minY = bottomRight.y;
        float maxY = topLeft.y;

        for (int r = 0; r < maxIterations; r++)
        {
            float currentSearchRadius = initialSearchRadius + r * radius * 2;

            for (int i = 0; i < steps; i++)
            {
                float angle = i * Mathf.PI * 2 / steps;
                Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                Vector2 checkPos = origin + dir * currentSearchRadius;

                if (!IsInsideMap(checkPos, radius, minX, maxX, minY, maxY))
                    continue;

                //Visualize checking spot
               // DrawCircle(checkPos, radius, Color.red);

                // Try to find free spot
                if (!Physics2D.OverlapCircle(checkPos, radius))
                {
                    // Move the circle to the new spot 
                    _collider.enabled = false;
                    _canMove = false;
                    Tween.Position(transform, checkPos, 0.1f, ease: Ease.InOutBounce).OnComplete(() =>
                    {
                        _rb.linearVelocity = dir * _speed;
                        _collider.enabled = true;
                        _canMove = true;
                    });
                    return;
                }
            }
        }

        // Do smth if can not find new spot
        // Kill circle
    }

    private bool IsInsideMap(Vector2 pos, float radius, float minX, float maxX, float minY, float maxY)
    {
        return
            pos.x - radius >= minX &&
            pos.x + radius <= maxX &&
            pos.y - radius >= minY &&
            pos.y + radius <= maxY;
    }


    /*private void DrawCircle(Vector2 center, float radius, Color color, int segments = 20)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector2(Mathf.Cos(0), Mathf.Sin(0)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

            Debug.DrawLine(prevPoint, nextPoint, color, 10f); // 1 секунда видно
            prevPoint = nextPoint;
        }
    }*/


    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (!_hasWeapon && collision.CompareTag("ShurikenWeapon"))
       {
            _hasWeapon = true;
            Destroy(collision.gameObject);
            // Play equip SFX
            MicroAudio.PlayEffectSound(LevelManager.instance.ShurikenEquipGroup.GetRandomClip);
            ShurikenWeapon shurikenWeapon = this.AddComponent<ShurikenWeapon>();
            shurikenWeapon.WeaponOwnerTransform = transform;
            shurikenWeapon.AttackLayer = _attackLayer;
            shurikenWeapon.CircleRadius = _collider.radius * transform.lossyScale.x;
            shurikenWeapon.ShurikenSpeed = LevelManager.instance.ShurikenSO.ShurikenSpeed;
            shurikenWeapon.CoolDown = LevelManager.instance.ShurikenSO.CoolDown;
            shurikenWeapon.Capacity = LevelManager.instance.ShurikenSO.Capacity;
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = LevelManager.instance.ShurikenSO.ShurikenSprite;
       }

       if (collision.CompareTag("Multiplayer"))
       {
            Destroy(collision.gameObject);
            MicroAudio.PlayEffectSound(LevelManager.instance.MultiplyGroup.GetRandomClip);

            Vector2 offset = Random.insideUnitCircle * 0.5f;
            Vector3 spawnPosition = transform.position + new Vector3(offset.x, offset.y, 0f);
            GameObject spawnedCircle = Instantiate(transform.gameObject, spawnPosition, Quaternion.identity);

        }
    }

    public void WaitToTakeNextWeapon(float coolDownTime)
    {
        StartCoroutine(CoolDownWeapon(coolDownTime));
    }

    private IEnumerator CoolDownWeapon(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        _hasWeapon = false;
    }

    public void OnDieCircle()
    {
        Instantiate(_dieParticlesPrefab, transform.position, _dieParticlesPrefab.transform.rotation);
        MicroAudio.PlayEffectSound(LevelManager.instance.OnDieGroup.GetRandomClip);
    }
}
