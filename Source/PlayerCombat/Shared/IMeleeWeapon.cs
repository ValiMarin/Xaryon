public interface IMeleeWeapon
{
    void ActivateAbility();
    void AttackStart();
    bool abilityInUse { get; }
    event System.Action<bool> OnAbilityStateChanged;
}
