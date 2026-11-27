using UnityEngine;

public class PenetratorBehaviour : MonoBehaviour
{
    [SerializeField] private PenetratorSpikes spikes;
    [SerializeField] private TurretDetection turretDetection;
    [SerializeField] private EnemyHealth turretHealth;

    private void OnEnable()
    {
        turretDetection.OnStandbyEnter += spikes.Extend;
        turretDetection.OnStandbyExit += spikes.Retract;
        turretHealth.OnDestroyed += spikes.DestroySpikes;
    }

    private void OnDisable()
    {
        turretDetection.OnStandbyEnter -= spikes.Extend;
        turretDetection.OnStandbyExit -= spikes.Retract;
        turretHealth.OnDestroyed -= spikes.DestroySpikes;
    }
}
