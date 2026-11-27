using UnityEngine;

public class SniperTurretSFX : TurretSFX
{
    [SerializeField] AudioClip bounceBullet, chargeup;

    [SerializeField] private TurretDetection turretDetection;

    private void OnEnable()
    {
        turretDetection.OnTargetLost += StopChargeup;
        turretDetection.OnStandbyEnter += StopChargeup;
    }

    private void OnDisable()
    {
        turretDetection.OnTargetLost -= StopChargeup;
        turretDetection.OnStandbyEnter -= StopChargeup;
    }

    public void PlayBounceBullet()
    {
        audioSource.clip = bounceBullet;
        audioSource.Play();
    }

    public void PlayChargeup()
    {
        audioSource.clip = chargeup;
        audioSource.Play();
    }

    public void StopChargeup()
    {
        if (audioSource.isPlaying && audioSource.clip == chargeup)
        {
            audioSource.Stop();
        }
    }
}
