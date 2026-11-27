using UnityEngine;

public class PistolAnimationHandler : MonoBehaviour
{
    private PistolController pistol;

    private Animator handAnimator;
    private Animator pistolAnim;

    private static readonly int ShootHash = Animator.StringToHash("Shoot");
    private static readonly int ShootProjectileHash = Animator.StringToHash("ShootProjectile");
    private static readonly int StopShootingHash = Animator.StringToHash("StopShooting");
    private static readonly int ReloadPistolHash = Animator.StringToHash("ReloadPistol");
    private static readonly int ReloadSMGHash = Animator.StringToHash("ReloadSMG");
    private static readonly int ReloadRocketLauncherHash = Animator.StringToHash("ReloadProjectile");
    private static readonly int RocketLauncherIdleHash = Animator.StringToHash("RocketLauncherIdle");

    private void Awake()
    {
        pistol = GetComponent<PistolController>();
    }

    private void Start()
    {
        pistolAnim = GetComponent<Animator>();
        handAnimator = GetComponentInParent<WeaponManager>().anim;
    }

    private void OnEnable()
    {
        if (pistol == null) return;

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnPauseStarted += OnPause;
            GameStateManager.Instance.OnPauseEnded += OnResume;
        }

        pistol.OnShoot += PlayShooting;
        pistol.OnReload += PlayReloading;
        pistol.OnModeSwitch += PlayModeSwitch;
        pistol.OnStopShooting += StopShooting;
    }

    private void OnDisable()
    {
        if (pistol == null) return;

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnPauseStarted -= OnPause;
            GameStateManager.Instance.OnPauseEnded -= OnResume;
        }

        pistol.OnShoot -= PlayShooting;
        pistol.OnReload -= PlayReloading;
        pistol.OnModeSwitch -= PlayModeSwitch;
        pistol.OnStopShooting -= StopShooting;
    }

    private void PlayShooting()
    {
        var mode = pistol.GetMode();

        if (mode == ShootingMode.Pistol)
        {
            handAnimator.Play(ShootHash, 0, 0f);
            pistolAnim.Play(ShootHash, 0, 0f);
        }
        else if (mode == ShootingMode.SMG)
        {
            handAnimator.Play(ShootHash, 0, 0f);
        }
        else if (mode == ShootingMode.Projectile)
        {
            handAnimator.SetTrigger(ShootProjectileHash);
        }
    }

    private void StopShooting()
    {
        handAnimator.SetTrigger(StopShootingHash);
    }

    private void PlayReloading()
    {
        var mode = pistol.GetMode();

        if (mode == ShootingMode.Pistol)
        {
            handAnimator.SetTrigger(ReloadPistolHash);
            pistolAnim.SetTrigger(ReloadPistolHash);
        }
        else if (mode == ShootingMode.SMG)
        {
            handAnimator.SetTrigger(ReloadSMGHash);
            pistolAnim.SetTrigger(ReloadSMGHash);
        }
        else if (mode == ShootingMode.Projectile)
        {
            pistolAnim.SetTrigger(ReloadRocketLauncherHash);
        }
    }

    private void PlayModeSwitch()
    {
        var mode = pistol.GetMode();

        if (mode == ShootingMode.Projectile)
        {
            handAnimator.SetBool(RocketLauncherIdleHash, true);
            return;
        }

        handAnimator.SetBool(RocketLauncherIdleHash, false);
    }

    private void OnPause()
    {
        pistolAnim.speed = 0f;
    }

    private void OnResume()
    {
        pistolAnim.speed = 1f;
    }
}