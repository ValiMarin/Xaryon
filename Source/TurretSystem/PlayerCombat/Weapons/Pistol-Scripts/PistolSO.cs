using UnityEngine;

[CreateAssetMenu(fileName = "PistolSO", menuName = "Scriptable Objects/PistolStatusSO")]
public class PistolSO : ScriptableObject
{
    [Header("Modes")]
    public bool hasPistol;
    public bool hasSMG;
    public bool hasProjectile;

    [System.Serializable]
    public class DamageRange
    {
        public int min;
        public int max;
    }

    [Header("Damage")]
    public DamageRange pistolDamage;
    public DamageRange smgDamage;

    [Header("Fire Rate")]
    public float pistolFireRate;
    public float smgFireRate;
    public float projectileFireRate;

    [Header("Recoil")]
    public Vector2 pistolRecoil;
    public Vector2 smgRecoil;

    [Header("Projectile")]
    public ProjectileSO projectileData;

    [System.Serializable]
    public class AmmoData
    {
        public int magazineSize;
    }

    [Header("Ammo")]
    public AmmoData pistolAmmo;
    public AmmoData smgAmmo;
    public AmmoData projectileAmmo;

    [Header("VFX")]
    public GameObject[] hitPrefabs;

    [Header("SFX")]
    public AudioClip pistolShoot;
    public AudioClip smgShoot;
    public AudioClip pistolReload;
    public AudioClip smgReload;
    public AudioClip projectileShoot;
    public AudioClip projectileReload;
    public AudioClip noAmmo;
    public AudioClip shootingMode;
}
