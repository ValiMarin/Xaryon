using UnityEngine;

public class RightHandAnimEventsHandler : MonoBehaviour
{
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private PlayerAbilities playerAbilities;
    
    private IWeapon currentWeapon;

    public void SetWeapon(IWeapon weapon)
    {
        currentWeapon = weapon;
    }

    public void DealDamage()
    {
        currentWeapon?.DealDamage();
    }

    public void ActivateAbility()
    {
        if (currentWeapon is IMeleeWeapon ability)
            ability.ActivateAbility();
    }

    public void OpenComboWindow()
    {
        if (currentWeapon is IComboWeapon combo)
            combo.OpenComboWindow();
    }

    public void CloseComboWindow()
    {
        if (currentWeapon is IComboWeapon combo)
            combo.CloseComboWindow();
    }

    public void AttackStart()
    {
        if (currentWeapon is IMeleeWeapon ability)
            ability.AttackStart();
    }

    public void ChangeWeapon()
    {
        weaponManager.ChangeWeapon();
    }

    public void DeadframeReady()
    {
        weaponManager.DeadframeReady();
    }

    public void DeadframeFinish()
    {
        weaponManager.DeadframeFinish();
    }

    public void ReloadComplete()
    {
        if (currentWeapon is IReloadable reloadable)
            reloadable.ReloadComplete();
    }

    public void DeadframeIdleStart()
    {
        playerAbilities.StartDeadframeIdle();
    }

    public void DeadframeExplosion()
    {
        playerAbilities.DeadframeExplosion();
    }
}
