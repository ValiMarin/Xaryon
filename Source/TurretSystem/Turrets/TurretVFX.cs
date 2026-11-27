using UnityEngine;

public class TurretVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem shoot;

    public void PlayShoot()
    {
        shoot.Play();
    }
}
