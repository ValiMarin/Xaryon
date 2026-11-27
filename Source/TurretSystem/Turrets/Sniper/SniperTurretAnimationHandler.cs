using UnityEngine;

public class SniperTurretAnimationHandler : TurretAnimationHandler
{
    [SerializeField] SniperTurretSFX sniperTurretSFX;

    public void Chargeup()
    {
        sniperTurretSFX.PlayChargeup();
    }
}
