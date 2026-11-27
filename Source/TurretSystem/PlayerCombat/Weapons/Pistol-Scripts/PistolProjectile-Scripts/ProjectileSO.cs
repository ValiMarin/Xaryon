using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSO", menuName = "Scriptable Objects/ProjectileStatusSO")]
public class ProjectileSO : ScriptableObject
{
    [Header("Prefab")]
    public GameObject prefab;

    [System.Serializable]
    public class DamageRange
    {
        public int min;
        public int max;
    }

    [Header("Damage")]
    public DamageRange projectileHit;
    public DamageRange projectileSplash;

    [Header("Characteristics")]
    public float speed;
    public float explosionRadius;
    public float lifetime = 5f;
}
