using UnityEngine;

public class PistolShooting : MonoBehaviour
{
    private PistolController pistol;
    private PistolVFX pistolVFX;
    private PistolRecoil recoil;

    [SerializeField] private PistolSO data;

    [SerializeField] private BulletTracer tracerPrefab;
    [SerializeField] private Transform PistolMuzzlePoint;
    [SerializeField] private Transform SMGMuzzlePoint;
    [SerializeField] private Transform ProjectileMuzzlePoint;

    private Transform cam;
    private PlayerLook playerLook;

    private const float RANGE = 1000f;
    private LayerMask pistolMask;

    private void Awake()
    {
        pistolMask = LayerMask.GetMask("Bullets", "AimAssist", "Player");
        cam = GetComponentInParent<Camera>().transform;
        playerLook = GetComponentInParent<PlayerLook>();
        pistol = GetComponent<PistolController>();
        pistolVFX = GetComponent<PistolVFX>();
        recoil = GetComponent<PistolRecoil>();

        playerLook.SetPistolRicoil(recoil);
    }

    public void DealDamage()
    {
        var mode = pistol.GetMode();

        switch (mode)
        {
            case ShootingMode.Pistol:
                Shoot(data.pistolDamage, data.pistolRecoil, PistolMuzzlePoint, pistolVFX.PlayPistolMuzzle);
                break;

            case ShootingMode.SMG:
                Shoot(data.smgDamage, data.smgRecoil, SMGMuzzlePoint, pistolVFX.PlaySMGMuzzle);
                break;

            case ShootingMode.Projectile:
                ShootProjectile();
                break;
        }

        pistol.OnBulletFired();
    }

    void Shoot(PistolSO.DamageRange dmg , Vector2 recoilData, Transform muzzle, System.Action playMuzzle)
    {
        Ray ray = new Ray(cam.position, cam.forward);
        Vector3 endPoint = cam.position + cam.forward * RANGE;

        if (Physics.Raycast(ray, out RaycastHit hit, RANGE, ~pistolMask))
        {
            endPoint = hit.point;

            if (hit.transform.GetComponentInParent<IDamageable>() is IDamageable turret)
            {
                int damage = Random.Range(dmg.min, dmg.max);
                turret.TakeDamage(damage);
            }

            pistolVFX.PlayHit(hit);
        }

        float minTracerDistance = 5f;
        float sqrDist = (endPoint - muzzle.position).sqrMagnitude;

        if (sqrDist > minTracerDistance * minTracerDistance)
        {
            SpawnTracer(endPoint, muzzle);
        }

        recoil.ApplyRecoil(recoilData);
        playMuzzle?.Invoke();
    }

    void ShootProjectile()
    {
        Ray ray = new Ray(cam.position, cam.forward);
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, RANGE, ~pistolMask))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = cam.position + cam.forward * RANGE;
        }

        Vector3 dir = (targetPoint - ProjectileMuzzlePoint.position).normalized;

        GameObject projectile = Instantiate(data.projectileData.prefab, ProjectileMuzzlePoint.position, Quaternion.LookRotation(dir));

        projectile.GetComponent<PistolProjectile>().Init(dir, data.projectileData);

        pistolVFX.PlayProjectileLauncherMuzzle();
    }

    void SpawnTracer(Vector3 endPoint, Transform muzzlePoint)
    {
        var tracer = Instantiate(tracerPrefab, muzzlePoint.position, Quaternion.identity);
        tracer.Init(muzzlePoint.position, endPoint);
    }
}