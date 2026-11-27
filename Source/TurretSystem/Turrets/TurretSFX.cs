using UnityEngine;

public class TurretSFX : MonoBehaviour
{
    protected AudioSource audioSource;
    private TurretSO data;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Init(TurretSO data)
    {
        this.data = data;
    }

    public void PlayDetection()
    {
        audioSource.PlayOneShot(data.detectationSound);
    }

    public void PlayShoot()
    {
        audioSource.PlayOneShot(data.shootSound);
    }

    public void PlayStandby()
    {
        audioSource.loop = true;
        audioSource.clip = data.standbySound;
        audioSource.Play();
    }

    public void StopStandby()
    {
        audioSource.loop = false;
        audioSource.Stop();
    }
}
