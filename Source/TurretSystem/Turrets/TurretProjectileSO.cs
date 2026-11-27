using UnityEngine;

[CreateAssetMenu(fileName = "TurretProjectileSO", menuName = "Scriptable Objects/TurretProjectileStatusSO")]
public class TurretProjectileSO : ScriptableObject
{
    [Header("Damage")]
    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;

    public int GetDamage()
    {
        return Random.Range(minDamage, maxDamage + 1);
    }

    public float speed;

    public Rigidbody projectilePrefab;
}
