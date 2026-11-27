using UnityEngine;

public class TurretShooting : MonoBehaviour
{
    private Transform target;

    private TurretProjectileSO projectileData;

    public void Init(TurretProjectileSO data, Transform target)
    {
        projectileData = data;
        this.target = target;
    }

    public virtual void Shoot()
    {
        Rigidbody prefab = Instantiate(projectileData.projectilePrefab, transform.position, transform.rotation);

        TurretProjectile projectile = prefab.GetComponent<TurretProjectile>();

        Vector3 velocity = CalculateVelocity(transform.position, target.position, projectileData.speed);

        int damage = projectileData.GetDamage();

        projectile.Init(damage, velocity, transform);
    }

    private Vector3 CalculateVelocity(Vector3 origin, Vector3 target, float speed)
    {
        Vector3 direction = target - origin;

        Vector3 horizontal = new Vector3(direction.x, 0f, direction.z);

        float distance = horizontal.magnitude;

        float time = distance / speed;

        float arcMultiplier = Mathf.Lerp(1f, 1.35f, Mathf.Clamp01(distance / 50f));
        time *= arcMultiplier;

        Vector3 velocityXZ = horizontal / time;

        float velocityY = (direction.y / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

        Vector3 velocity = velocityXZ;
        velocity.y = velocityY;

        return velocity;
    }
}