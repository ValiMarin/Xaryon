using UnityEngine;

public class PenetratorSpikes : MonoBehaviour
{
    private Animator anim;

    private SpikesSFX spikesSFX;

    private static readonly int ExtendedHash = Animator.StringToHash("Extended");

    private void Awake()
    {
        spikesSFX = GetComponent<SpikesSFX>();
        anim = GetComponent<Animator>();
    }

    public void Extend()
    {
        anim.SetBool(ExtendedHash, true);
        spikesSFX.PlayExtend();
    }

    public void Retract()
    {
        anim.SetBool(ExtendedHash, false);
        spikesSFX.PlayRetract();
    }

    public void DestroySpikes()
    {
        Destroy(gameObject);
    }
}
