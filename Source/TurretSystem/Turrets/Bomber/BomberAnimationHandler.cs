using UnityEngine;

public class BomberAnimationHandler : TurretAnimationHandler
{
    [SerializeField] private BomberAbility bomberAbility;
    [SerializeField] private BomberSFX bomberSFX;

    private static readonly int AbilityHash = Animator.StringToHash("Ability");

    protected override void ApplyProfile()
    {
        base.ApplyProfile();
        anim.SetFloat("AbilitySpeed", profile.abilitySpeed);
    }

    public void FireAbility()
    {
        bomberAbility.FireAbility();
    }

    public void AudioAbility()
    {
        bomberSFX.PlayAbility();
    }

    public void CheckAbilityChance()
    {
        if (bomberAbility.TryConsumeAbility())
            anim.SetTrigger(AbilityHash);
    }
}
