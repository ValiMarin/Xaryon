using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections.Generic;

public class PistolController : MonoBehaviour, IWeapon, IReloadable
{
    private PlayerControls inputActions;
    private PlayerAmmo playerAmmo;
    private PistolShooting pistolShooting;
    private PlayerMovement playerMove;

    [SerializeField] private ProjectileLaser laser;
    [SerializeField] private PistolSO data;
    public PistolSO Data => data;

    public event Action OnShoot;
    public event Action OnReload;
    public event Action OnModeSwitch;
    public event Action OnStopShooting;
    public event Action<ShootingMode, int, int> OnAmmoChanged;
    public event Action OnNoAmmo;

    private List<ShootingMode> availableModes = new();
    private int currentModeIndex;

    private Dictionary<ShootingMode, int> currentAmmo = new();
    private Dictionary<ShootingMode, int> maxAmmo = new();

    private bool isReloading;
    private bool isShooting;
    private bool justFiredLastBullet;
    private bool isSprinting;

    private float nextFireTime = 0;

    #region INIT
    private void Awake()
    {
        inputActions = InputManager.Instance.Controls;
        pistolShooting = GetComponent<PistolShooting>();
        playerAmmo = GetComponentInParent<PlayerAmmo>();
    }

    public void Initialize()
    {
        SetupModes();
        SetupAmmo();

        AutoReloadAllModes();
        UpdateLaser();
    }
    #endregion

    #region SETUP MODES
    private void SetupModes()
    {
        availableModes.Clear();

        if (data.hasPistol)
            availableModes.Add(ShootingMode.Pistol);

        if (data.hasSMG)
            availableModes.Add(ShootingMode.SMG);

        if (data.hasProjectile)
            availableModes.Add(ShootingMode.Projectile);
    }

    private void SetupAmmo()
    {
        if (data.hasPistol)
        {
            maxAmmo[ShootingMode.Pistol] = data.pistolAmmo.magazineSize;
            currentAmmo[ShootingMode.Pistol] = data.pistolAmmo.magazineSize;
        }

        if (data.hasSMG)
        {
            maxAmmo[ShootingMode.SMG] = data.smgAmmo.magazineSize;
            currentAmmo[ShootingMode.SMG] = data.smgAmmo.magazineSize;
        }

        if (data.hasProjectile)
        {
            maxAmmo[ShootingMode.Projectile] = data.projectileAmmo.magazineSize;
            currentAmmo[ShootingMode.Projectile] = data.projectileAmmo.magazineSize;
        }
    }
    #endregion

    #region INPUT
    private void OnEnable()
    {
        playerMove = PlayerMovement.PlayerMove;

        if (playerMove != null)
            playerMove.OnSprint += HandleSprint;

        if (playerAmmo != null)
            playerAmmo.OnAddAmmo += HandleAmmoAdded;

        inputActions.Player.Attack.started += OnShootStart;
        inputActions.Player.Attack.canceled += OnShootStop;

        inputActions.Player.Reload.performed += OnReloadInput;
        inputActions.Player.ShootingMode.performed += OnSwitchMode;
    }

    private void OnDisable()
    {
        inputActions.Player.Attack.started -= OnShootStart;
        inputActions.Player.Attack.canceled -= OnShootStop;

        inputActions.Player.Reload.performed -= OnReloadInput;
        inputActions.Player.ShootingMode.performed -= OnSwitchMode;

        if (playerMove != null)
            playerMove.OnSprint -= HandleSprint;

        if (playerAmmo != null)
            playerAmmo.OnAddAmmo -= HandleAmmoAdded;
    }

    private void OnShootStart(InputAction.CallbackContext ctx)
    {
        if (isSprinting) return;

        isShooting = true;
    }

    private void OnShootStop(InputAction.CallbackContext ctx)
    {
        isShooting = false;
        OnStopShooting?.Invoke();
    }

    private void OnReloadInput(InputAction.CallbackContext ctx)
    {
        if (isSprinting) return;

        TryReload();
    }

    private void OnSwitchMode(InputAction.CallbackContext ctx)
    {
        if (isReloading || isSprinting) return;

        currentModeIndex = (currentModeIndex + 1) % availableModes.Count;
        OnModeSwitch?.Invoke();
        UpdateLaser();
        TryAutoReload(GetMode());
    }
    #endregion

    private void Update()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsPaused)
            return;

        if (nextFireTime > 0)
            nextFireTime -= Time.unscaledDeltaTime;

        if (!isShooting) return;

        var mode = GetMode();

        if (currentAmmo[mode] <= 0)
        {
            if (playerAmmo.GetAmmo(mode) <= 0)
            {
                if (!justFiredLastBullet)
                {
                    OnNoAmmo?.Invoke();
                }

                justFiredLastBullet = false;
                isShooting = false;
                return;
            }

            TryReload();
            justFiredLastBullet = false;
            return;
        }

        justFiredLastBullet = false;

        TryShoot();
    }

    #region SHOOT LOGIC
    private void TryShoot()
    {
        var mode = GetMode();

        if (currentAmmo[mode] <= 0 && playerAmmo.GetAmmo(mode) <= 0)
        {
            OnNoAmmo?.Invoke();
            return;
        }

        if (!CanShoot()) return;

        nextFireTime = GetFireRate();

        ApplyReticleKick();

        OnShoot?.Invoke();
    }

    private bool CanShoot()
    {
        if (isReloading) return false;

        if (nextFireTime > 0) return false;

        if (GetCurrentAmmo() <= 0) return false;

        return true;
    }

    public void OnBulletFired()
    {
        var mode = GetMode();

        currentAmmo[mode]--;

        justFiredLastBullet = currentAmmo[mode] <= 0;

        NotifyAmmo(mode);

        if (currentAmmo[mode] <= 0)
        {
            OnStopShooting?.Invoke();
            Invoke(nameof(TryReload), 0.15f);
        }
    }

    private float GetFireRate()
    {
        return GetMode() switch
        {
            ShootingMode.Pistol => data.pistolFireRate,
            ShootingMode.SMG => data.smgFireRate,
            ShootingMode.Projectile => data.projectileFireRate,
            _ => 0.5f
        };
    }

    private void UpdateLaser()
    {
        var mode = GetMode();

        bool showLaser = (mode == ShootingMode.Projectile);

        if (laser != null)
            laser.SetActive(showLaser);
    }

    private void ApplyReticleKick()
    {
        float kick = GetMode() switch
        {
            ShootingMode.Pistol => 20f,
            ShootingMode.SMG => 10f,
            ShootingMode.Projectile => 40f,
            _ => 10f
        };

        GameEvents.ReticleKick(kick);
    }
    #endregion

    #region AMMO
    private void TryReload()
    {
        var mode = GetMode();

        if (isReloading)
            return;

        if (currentAmmo[mode] >= maxAmmo[mode])
            return;

        if (!playerAmmo.HasAmmo(mode))
            return;

        isShooting = false;
        OnStopShooting?.Invoke();

        isReloading = true;
        OnReload?.Invoke();
    }

    public void ReloadComplete()
    {
        var mode = GetMode();

        int needed = maxAmmo[mode] - currentAmmo[mode];
        int taken = playerAmmo.TakeAmmo(mode, needed);

        currentAmmo[mode] += taken;

        isReloading = false;
        NotifyAmmo(GetMode());
    }

    private void AutoReloadAllModes()
    {
        foreach (var mode in availableModes)
        {
            int taken = playerAmmo.TakeAmmo(mode, maxAmmo[mode]);
            currentAmmo[mode] = taken;

            NotifyAmmo(mode);
        }
    }

    private void NotifyAmmo(ShootingMode mode)
    {
        OnAmmoChanged?.Invoke(mode, currentAmmo[mode], playerAmmo.GetAmmo(mode));
    }

    private void HandleAmmoAdded(ShootingMode mode)
    {
        TryAutoReload(mode);
    }

    private void TryAutoReload(ShootingMode mode)
    {
        if (mode != GetMode())
            return;

        if (isReloading)
            return;

        if (currentAmmo[mode] > 0)
            return;

        if (!playerAmmo.HasAmmo(mode))
            return;

        TryReload();
    }
    #endregion

    #region API
    public ShootingMode GetMode()
    {
        return availableModes[currentModeIndex];
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo[GetMode()];
    }

    public void DealDamage()
    {
        pistolShooting.DealDamage();
    }

    private void HandleSprint(bool isRunning)
    {
        isSprinting = isRunning;

        if (isRunning)
        {
            if (isShooting)
            {
                isShooting = false;
                OnStopShooting?.Invoke();
            }

            if (isReloading)
            {
                isReloading = false;
                OnStopShooting?.Invoke();
            }
        }
    }
    #endregion
}