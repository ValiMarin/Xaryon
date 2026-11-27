using UnityEngine;

public class PistolSFX : MonoBehaviour
{
    private AudioSource audioSource;

    private PistolController pistol;
    private PistolSO data;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        pistol = GetComponent<PistolController>();

        data = pistol.Data;
    }

    private void OnEnable()
    {
        if (pistol == null) return;

        pistol.OnModeSwitch += PlayModeSwitch;
        pistol.OnReload += PlayReloading;
        pistol.OnShoot += PlayShooting;
        pistol.OnNoAmmo += PlayNoAmmo;
    }

    private void OnDisable()
    {
        if (pistol == null) return;

        pistol.OnModeSwitch -= PlayModeSwitch;
        pistol.OnReload -= PlayReloading;
        pistol.OnShoot -= PlayShooting;
        pistol.OnNoAmmo -= PlayNoAmmo;
    }

    private void PlayModeSwitch()
    {
        audioSource.PlayOneShot(data.shootingMode);
    }

    private void PlayReloading()
    {
        var mode = pistol.GetMode();

        if (mode == ShootingMode.Pistol)
        {
            audioSource.PlayOneShot(data.pistolReload);
        }
        else if (mode == ShootingMode.SMG)
        {
            audioSource.PlayOneShot(data.smgReload);
        }
        else if (mode == ShootingMode.Projectile)
        {
            audioSource.PlayOneShot(data.projectileReload);
        }
    }

    private void PlayShooting()
    {
        var mode = pistol.GetMode();

        if (mode == ShootingMode.Pistol)
        {
            audioSource.PlayOneShot(data.pistolShoot);
        }
        else if (mode == ShootingMode.SMG)
        {
            audioSource.PlayOneShot(data.smgShoot);
        }
        else if (mode == ShootingMode.Projectile)
        {
            audioSource.PlayOneShot(data.projectileShoot);
        }
    }

    private void PlayNoAmmo()
    {
        audioSource.PlayOneShot(data.noAmmo);
    }
}
