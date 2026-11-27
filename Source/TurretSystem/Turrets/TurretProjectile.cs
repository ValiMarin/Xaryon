using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    protected Rigidbody rb;

    protected int damage;

    [SerializeField] private GameObject explosionPrefab;

    protected Transform shooter;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Init(int damage, Vector3 velocity, Transform shooter)
    {
        this.damage = damage;
        this.shooter = shooter;

        rb.linearVelocity = velocity;
    }

    protected virtual void Update()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            transform.forward = rb.linearVelocity.normalized;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponentInParent<IDamageable>() is IDamageable target)
        {
            target.TakeDamage(damage);

            if (collision.transform.CompareTag("Player"))
            {
                DI_System.CreateIndicator(shooter);
            }
        }

        SpawnExplosion();

        Destroy(gameObject);
    }

    protected virtual void SpawnExplosion()
    {
        if (explosionPrefab == null)
            return;

        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);

        Destroy(explosion, 2f);
    }
}