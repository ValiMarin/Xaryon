using UnityEngine;

public class SniperTurretShooting : TurretShooting
{
    [SerializeField] private GameObject tracerPrefab;

    [SerializeField] SniperTurretSFX sniperTurretSFX;

    private const int DAMAGE_MIN = 7;
    private const int DAMAGE_MAX = 12;

    public override void Shoot()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f))
            return;

        SpawnTracer(hit.point);

        if (hit.transform.CompareTag("Player"))
        {
            int damage = Random.Range(DAMAGE_MIN, DAMAGE_MAX + 1);

            hit.transform.GetComponentInParent<IDamageable>()?.TakeDamage(damage);

            DI_System.CreateIndicator(transform);
        }
        else if (hit.transform.CompareTag("Shield"))
        {
            hit.transform.GetComponentInParent<IDamageable>()?.TakeDamage(0);
            sniperTurretSFX.PlayBounceBullet();
        }
    }

    private void SpawnTracer(Vector3 hitPoint)
    {
        if (tracerPrefab == null)
            return;

        GameObject tracer = Instantiate(tracerPrefab, hitPoint, transform.rotation);

        Destroy(tracer, 2f);
    }
}