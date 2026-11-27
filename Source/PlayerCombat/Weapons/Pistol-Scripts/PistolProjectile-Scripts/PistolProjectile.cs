using System.Collections.Generic;
using UnityEngine;

public class PistolProjectile : MonoBehaviour
{
    private ProjectileSO data;
    private Vector3 direction;

    private float speed;
    private bool initialized;

    [SerializeField] private GameObject projectileExplosion;
    [SerializeField] private GameObject effectHolder;
    [SerializeField] private ParticleSystem smokeTrail;

    private LayerMask bulletMask;

    public void Init(Vector3 dir, ProjectileSO projectileData)
    {
        data = projectileData;
        direction = dir.normalized;
        speed = data.speed;

        initialized = true;

        bulletMask = LayerMask.GetMask("Bullets", "AimAssist", "Player");

        Destroy(gameObject, data.lifetime);
    }

    private void Update()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsPaused)
            return;

        if (!initialized) return;

        float move = speed * Time.unscaledDeltaTime;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, move + 0.3f, ~bulletMask))
        {
            Impact(hit);
            return;
        }

        transform.position += direction * move;
    }

    private void Impact(RaycastHit hit)
    {
        Vector3 point = hit.point;

        if (projectileExplosion != null)
        {
            projectileExplosion.SetActive(true);
            smokeTrail.Stop();
            effectHolder.transform.parent = null;
            Destroy(effectHolder, 2f);
        }

        var directHit = hit.transform.GetComponentInParent<IDamageable>();

        HashSet<IDamageable> damaged = new();

        if (directHit != null)
        {
            int damage = Random.Range(data.projectileHit.min, data.projectileHit.max);
            directHit.TakeDamage(damage);
            damaged.Add(directHit);
        }

        Collider[] hits = Physics.OverlapSphere(point, data.explosionRadius, ~0, QueryTriggerInteraction.Collide);

        foreach (var col in hits)
        {
            var dmgable = col.GetComponentInParent<IDamageable>();
            if (dmgable == null || damaged.Contains(dmgable))
                continue;

            var comp = dmgable as Component;
            if (comp == null) continue;

            Transform root = comp.transform;

            Vector3 closest = col.ClosestPoint(point);
            Vector3 dir = closest - point;

            float dist = dir.magnitude;
            if (dist <= 0.01f)
                continue;

            dir.Normalize();

            Vector3 origin = point + dir * 0.01f;

            if (Physics.Raycast(origin, dir, out RaycastHit blockHit, dist, ~bulletMask))
            {
                if (!blockHit.transform.IsChildOf(root))
                    continue;
            }

            float t = dist / data.explosionRadius;
            float falloff = Mathf.Pow(1f - t, 2f);

            int dmg = Mathf.RoundToInt(Mathf.Lerp(data.projectileSplash.min, data.projectileSplash.max, falloff));

            dmgable.TakeDamage(dmg);
            damaged.Add(dmgable);
        }

        Destroy(gameObject);
    }
}