using UnityEngine;

public class BomberController : TurretController
{
    [SerializeField] private Transform leftBombPosition;
    [SerializeField] private Transform rightBombPosition;

    [SerializeField] private BomberProjectile bombPrefab;

    private BomberAbility bomberAbility;

    protected override void Awake()
    {
        base.Awake();
        bomberAbility = GetComponent<BomberAbility>();
        bomberAbility.GetPlayerPosition(target);
    }

    protected override void HandleDeath()
    {
        SpawnBomb(leftBombPosition, -transform.right * 20f);
        SpawnBomb(rightBombPosition, transform.right * 20f);

        base.HandleDeath();
    }

    private void SpawnBomb(Transform spawnPoint, Vector3 force)
    {
        BomberProjectile bomb = Instantiate(bombPrefab, spawnPoint.position, spawnPoint.rotation);

        bomb.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }
}
