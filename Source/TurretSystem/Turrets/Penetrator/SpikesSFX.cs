using UnityEngine;

public class SpikesSFX : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip extendClip;
    [SerializeField] private AudioClip retractClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayExtend()
    {
        audioSource.PlayOneShot(extendClip);
    }

    public void PlayRetract()
    {
        audioSource.PlayOneShot(retractClip);
    }
}
