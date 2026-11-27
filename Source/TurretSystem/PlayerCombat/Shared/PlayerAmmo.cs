using UnityEngine;
using System;

public class PlayerAmmo : MonoBehaviour
{
    [SerializeField] private int pistolAmmo = 20;
    [SerializeField] private int smgAmmo = 30;
    [SerializeField] private int projectileAmmo = 3;

    public event Action<ShootingMode> OnAddAmmo;

    #region GETTERS
    public int GetAmmo(ShootingMode mode)
    {
        switch (mode)
        {
            case ShootingMode.Pistol:
                return pistolAmmo;

            case ShootingMode.SMG:
                return smgAmmo;

            case ShootingMode.Projectile:
                return projectileAmmo;
        }

        return 0;
    }

    #endregion

    #region CONSUME

    public int TakeAmmo(ShootingMode mode, int amount)
    {
        int taken = 0;

        switch (mode)
        {
            case ShootingMode.Pistol:
                taken = Mathf.Min(pistolAmmo, amount);
                pistolAmmo -= taken;
                break;

            case ShootingMode.SMG:
                taken = Mathf.Min(smgAmmo, amount);
                smgAmmo -= taken;
                break;

            case ShootingMode.Projectile:
                taken = Mathf.Min(projectileAmmo, amount);
                projectileAmmo -= taken;
                break;
        }

        return taken;
    }

    #endregion

    #region ADD

    public void AddAmmo(ShootingMode mode, int amount)
    {
        switch (mode)
        {
            case ShootingMode.Pistol:
                pistolAmmo += amount;
                break;

            case ShootingMode.SMG:
                smgAmmo += amount;
                break;

            case ShootingMode.Projectile:
                projectileAmmo += amount;
                break;
        }

        OnAddAmmo?.Invoke(mode);
    }

    #endregion

    #region CHECK

    public bool HasAmmo(ShootingMode mode)
    {
        return GetAmmo(mode) > 0;
    }

    #endregion
}