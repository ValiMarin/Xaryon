using UnityEngine;

[CreateAssetMenu(fileName = "TurretSO", menuName = "Scriptable Objects/TurretStatusSO")]
public class TurretSO : ScriptableObject
{
    [Header("Detection")]
    public int detectionRange;
    public int intimidationRange;

    public int maxHealth;
    public int value;

    public TurretProjectileSO projectileData;

    public GameObject turretExplosion;

    [Header("SFX")]
    public AudioClip shootSound;
    public AudioClip detectationSound;
    public AudioClip standbySound;
}
