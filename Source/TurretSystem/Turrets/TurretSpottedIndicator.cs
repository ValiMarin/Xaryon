using UnityEngine;

public class TurretSpottedIndicator : MonoBehaviour
{
    [SerializeField] private TurretDetection detection;

    [SerializeField] private GameObject indicator;

    private bool hasEverShot;

    private Transform player;

    public void Init(Transform player)
    {
        this.player = player;
    }

    public void TurretShooted()
    {
        hasEverShot = true;
    }

    private void Update()
    {
        if (!hasEverShot)
        {
            indicator.SetActive(false);
            return;
        }

        if (player != null)
            indicator.transform.LookAt(player);

        indicator.SetActive(detection.turretSpotted);
    }
}