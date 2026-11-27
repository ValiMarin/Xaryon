using UnityEngine;

public class PenetratorProjectile : TurretProjectile
{
    private const float PENETRATION_CHANCE = 0.3f;

    [SerializeField] private float stuckLifetime = 5f;

    private bool stuck;
    private bool consumed;

    private Quaternion lastFlightRotation;

    protected override void Update()
    {
        if (!stuck && rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            transform.forward = rb.linearVelocity.normalized;
            lastFlightRotation = transform.rotation;
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (consumed)
            return;

        consumed = true;

        rb.detectCollisions = false;
        GetComponent<Collider>().enabled = false;

        HandleHit(collision);

        SpawnExplosion();
    }

    private void HandleHit(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            HitPlayer(collision);
            return;
        }

        if (collision.transform.CompareTag("Shield"))
        {
            HitShield(collision);
            return;
        }

        Stick(collision);
    }

    private void HitPlayer(Collision collision)
    {
        collision.transform.GetComponentInParent<IDamageable>()?.TakeDamage(damage);

        DI_System.CreateIndicator(shooter);

        Destroy(gameObject);
    }

    private void HitShield(Collision collision)
    {
        collision.transform.GetComponentInParent<IDamageable>()?.TakeDamage(damage);

        bool penetrated = Random.value <= PENETRATION_CHANCE;

        if (penetrated)
        {
            PlayerStatus.Player.TakeDamage(Mathf.CeilToInt(damage * 0.5f));

            DI_System.CreateIndicator(shooter);
        }

        Destroy(gameObject);
    }

    private void Stick(Collision collision)
    {
        stuck = true;

        transform.rotation = lastFlightRotation;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        Destroy(gameObject, stuckLifetime);
    }
}