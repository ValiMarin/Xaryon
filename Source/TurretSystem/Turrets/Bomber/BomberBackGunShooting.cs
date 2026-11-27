using UnityEngine;

public class BomberBackGunShooting : MonoBehaviour
{
    [SerializeField] private BomberProjectile projectilePrefab;
    [SerializeField] private float projectileSpeed = 15f;

    public void Shoot(Vector3 targetPosition)
    {
        BomberProjectile projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

        Vector3 velocity = CalculateVelocity(transform.position, targetPosition, projectileSpeed);

        projectile.Init(9, velocity, transform);
    }

    private Vector3 CalculateVelocity(Vector3 origin, Vector3 target, float speed)
    {
        Vector3 direction = target - origin;

        Vector3 horizontal = new Vector3(direction.x, 0f, direction.z);

        float distance = horizontal.magnitude;

        float time = distance / speed;

        float arcMultiplier =
            Mathf.Lerp(1f, 1.35f, Mathf.Clamp01(distance / 50f));

        time *= arcMultiplier;

        Vector3 velocityXZ = horizontal / time;

        float velocityY =
            (direction.y / time)
            + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

        Vector3 velocity = velocityXZ;
        velocity.y = velocityY;

        return velocity;
    }
}