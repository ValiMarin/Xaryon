using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    private Transform cam;

    private SwordStatusSO swordStatus;

    private SwordSFX swordSFX;
    private SwordVFX swordVFX;

    private LayerMask attackMask;

    private void Awake()
    {
        cam = GetComponentInParent<Camera>().transform;
        swordSFX = GetComponent<SwordSFX>();
        swordVFX = GetComponent<SwordVFX>();

        attackMask = LayerMask.GetMask("Player", "Bullets", "AimAssist");
    }

    public void InitializeAttack(SwordStatusSO swordData)
    {
        swordStatus = swordData;
    }

    public void DealDamage()
    {
        Ray ray = new Ray(cam.position, cam.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, swordStatus.basicRange, ~attackMask))
        {
            if (hit.transform.GetComponentInParent<IDamageable>() is IDamageable turret)
            {
                int damage = Random.Range(swordStatus.minDamage, swordStatus.maxDamage + 1);
                turret.TakeDamage(damage);
            }

            swordSFX.PlayHit(hit.transform.tag);
            swordVFX.PlayHit(hit);
        }
    }

    public void AttackStart()
    {
        swordSFX.PlayAttack();
        swordVFX.PlayAttack();
    }
}