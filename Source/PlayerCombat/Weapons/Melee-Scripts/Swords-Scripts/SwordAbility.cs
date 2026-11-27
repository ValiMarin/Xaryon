using System;
using UnityEngine;

public class SwordAbility : MonoBehaviour
{
    private SwordStatusSO swordStatus;
    private SwordDash swordDash;

    private int remainingTargets;

    private float cooldown;
    public float CooldownRemaining => cooldown;

    private ISuperEnergyUser energyUser;

    private bool abilityInAction = false;
    public bool AbilityInAction => abilityInAction;

    private SwordAbilityUI swordAbilityUI;

    public void InitializeAbility(SwordStatusSO status, ISuperEnergyUser user)
    {
        swordStatus = status;
        swordDash = GetComponent<SwordDash>();

        remainingTargets = swordStatus.abilityTargets;

        energyUser = user;

        cooldown = 0f;

        swordAbilityUI = FindFirstObjectByType<SwordAbilityUI>();

        swordAbilityUI.Init(this);
    }

    private void Update()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsPaused)
            return;

        if (cooldown > 0f)
            cooldown -= Time.unscaledDeltaTime;
    }

    public bool CanUseAbility()
    {
        return CooldownRemaining <= 0f && energyUser.CurrentSuperEnergy >= swordStatus.superEnergyCost;
    }

    public void ActivateAbility()
    {
        if (swordDash.CurrentTarget != null)
            swordDash.CurrentTarget.TakeDamage(swordStatus.abilityDamage);

        --remainingTargets;
        abilityInAction = true;
    }

    public void AbilityCost()
    {
        cooldown = swordStatus.abilityCooldown;
        energyUser.ConsumeSuperEnergy(swordStatus.superEnergyCost);
    }

    public bool AbilityFinished()
    {
        return remainingTargets <= 0;
    }

    public void ResetTargets()
    {
        remainingTargets = swordStatus.abilityTargets;
        abilityInAction = false;
    }
}