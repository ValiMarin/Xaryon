using UnityEngine;

public class BomberSFX : TurretSFX
{
    [SerializeField] private AudioClip abilityShoot;

    public void PlayAbility()
    {
        audioSource.PlayOneShot(abilityShoot);
    }
}
