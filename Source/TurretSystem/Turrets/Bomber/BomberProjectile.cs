using UnityEngine;
using System.Collections.Generic;

public class BomberProjectile : TurretProjectile
{
    private const int EXPLOSION_DELAY = 2;
    private const int EXPLOSION_RADIUS = 8;

    [SerializeField] private ParticleSystem armedAura;

    private bool armed;
    private bool playerHit;

    private LayerMask bombMask;

    private AudioSource audioSource;

    private void Start()
    {
        bombMask = LayerMask.GetMask("Bullets", "AimAssist", "Turret");
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        // intentionally empty
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (armed)
            return;

        ActivateBomb();
    }

    private void ActivateBomb()
    {
        armed = true;

        if (armedAura != null)
            armedAura.Play();

        audioSource.Play();

        Invoke(nameof(Explode), EXPLOSION_DELAY);
    }

    private void Explode()
    {
        Vector3 origin = transform.position;

        SpawnExplosion();

        Collider[] hits = Physics.OverlapSphere(origin, EXPLOSION_RADIUS);

        HashSet<IDamageable> damaged = new();

        foreach (Collider col in hits)
        {
            IDamageable dmg = col.GetComponentInParent<IDamageable>();
            if (dmg == null || damaged.Contains(dmg))
                continue;

            if (((1 << col.gameObject.layer) & ~bombMask) == 0)
                continue;

            Component comp = dmg as Component;
            if (comp == null)
                continue;

            Transform root = comp.transform;

            Vector3 closest = col.ClosestPoint(origin);
            Vector3 dir = closest - origin;

            float dist = dir.magnitude;
            if (dist <= 0.01f)
                continue;

            dir.Normalize();

            Vector3 rayOrigin = origin + dir * 0.1f;

            if (Physics.Raycast(rayOrigin, dir, out RaycastHit hit, dist, ~bombMask))
            {
                IDamageable hitDamageable = hit.transform.GetComponentInParent<IDamageable>();

                if (hitDamageable != dmg)
                    continue;
            }

            float t = dist / EXPLOSION_RADIUS;
            float falloff = Mathf.Pow(1f - t, 1.5f);

            int finalDamage = Mathf.RoundToInt(damage * falloff);

            dmg.TakeDamage(finalDamage);
            damaged.Add(dmg);

            if (comp.CompareTag("Player") && !playerHit)
            {
                DI_System.CreateIndicator(transform);
                playerHit = true;
            }
        }

        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EXPLOSION_RADIUS);
    }
#endif
}