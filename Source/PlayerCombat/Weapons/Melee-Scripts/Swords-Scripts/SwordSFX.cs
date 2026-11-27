using UnityEngine;

public class SwordSFX : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Attack")]
    [SerializeField] private AudioClip attack;

    [Header("Ability")]
    [SerializeField] private AudioClip slash;

    [Header("Hit Sounds")]
    [SerializeField] private AudioClip woodHit;
    [SerializeField] private AudioClip metalHit;
    [SerializeField] private AudioClip groundHit;
    [SerializeField] private AudioClip turretHit;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAttack()
    {
        audioSource.PlayOneShot(attack);
    }

    public void PlayHit(string surfaceType)
    {
        AudioClip hitClip = GetHitClip(surfaceType);

        if (hitClip != null)
            audioSource.PlayOneShot(hitClip);
    }

    private AudioClip GetHitClip(string surfaceType)
    {
        switch (surfaceType)
        {
            case "woodenOBJ":
                return woodHit;

            case "metalOBJ":
                return metalHit;

            case "Untagged":
            case "terrain":
                return groundHit;

            case "turret":
                return turretHit;

            default:
                return null;
        }
    }

    public void PlayAbilityIdle()
    {
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopAbilityIdle()
    {
        audioSource.loop = false;
        audioSource.Stop();
    }

    public void PlaySlash()
    {
        audioSource.PlayOneShot(slash);
    }
}