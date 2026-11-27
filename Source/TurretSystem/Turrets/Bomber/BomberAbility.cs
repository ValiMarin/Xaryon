using UnityEngine;

public class BomberAbility : MonoBehaviour
{
    private int abilityChance = 0;

    [SerializeField] private BomberBackGunShooting leftGun;
    [SerializeField] private BomberBackGunShooting centerGun;
    [SerializeField] private BomberBackGunShooting rightGun;

    [SerializeField] private ParticleSystem muzzleBackGuns;

    private Transform playerPosition;

    public void GetPlayerPosition(Transform playerPosition)
    {
        this.playerPosition = playerPosition;
    }

    public bool TryConsumeAbility()
    {
        ++abilityChance;

        if (Random.Range(0, 10) < abilityChance)
        {
            abilityChance = 0;
            return true;
        }

        return false;
    }

    public void FireAbility()
    {
        Vector3 center = playerPosition.position;

        Vector3 offset = playerPosition.right * 6f;

        leftGun.Shoot(center + offset);
        centerGun.Shoot(center);
        rightGun.Shoot(center - offset);

        muzzleBackGuns.Play();
    }
}
