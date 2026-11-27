using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private TurretSO turretData;

    [SerializeField] private TurretDetection turretDetection;
    [SerializeField] private TurretShooting turretShooting;
    [SerializeField] private TurretAnimationHandler turretAnim;
    [SerializeField] private TurretIndicator turretIndicator;
    [SerializeField] private TurretHPBar hpBar;

    private TurretSFX turretSFX;
    private TurretVFX turretVFX;
    private EnemyHealth turretHealth;
    private TurretHightlight turretHightlight;
    private TurretSpottedIndicator turretSpottedIndicator;

    [SerializeField] protected Transform target;

    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private Transform damagePopupPoint;

    [SerializeField] private LootDropper[] lootDroppers;

    private bool hasTarget;
    private bool isStandby;

    private enum TurretState
    {
        Idle,
        Shooting,
        Standby
    }

    private TurretState currentState;

    [SerializeField] private bool disableOnDeath;

    protected virtual void Awake()
    {
        turretSFX = GetComponent<TurretSFX>();
        turretVFX = GetComponent<TurretVFX>();
        turretHealth = GetComponent<EnemyHealth>();
        turretHightlight = GetComponent<TurretHightlight>();
        turretSpottedIndicator = GetComponent<TurretSpottedIndicator>();

        turretDetection.Init(target, turretData.detectionRange, turretData.intimidationRange);
        turretShooting.Init(turretData.projectileData, target);
        turretSFX.Init(turretData);
        turretHealth.Init(turretData.maxHealth);
        turretHightlight.Init(target);
        hpBar.Init(target);
        turretSpottedIndicator.Init(target);
    }

    private void OnEnable()
    {
        turretDetection.OnTargetDetected += TargetDetected;
        turretDetection.OnTargetLost += TargetLost;

        turretDetection.OnStandbyEnter += EnterStandby;
        turretDetection.OnStandbyExit += ExitStandby;

        turretHealth.OnDamageTaken += SpawnDamagePopup;
        turretHealth.OnDestroyed += HandleDeath;

        turretAnim.OnShootEvent += Shoot;
    }

    private void OnDisable()
    {
        turretDetection.OnTargetDetected -= TargetDetected;
        turretDetection.OnTargetLost -= TargetLost;
        turretDetection.ResetDetection();
        SetState(TurretState.Idle);
        hasTarget = false;
        isStandby = false;

        turretDetection.OnStandbyEnter -= EnterStandby;
        turretDetection.OnStandbyExit -= ExitStandby;

        turretHealth.OnDamageTaken -= SpawnDamagePopup;
        turretHealth.OnDestroyed -= HandleDeath;

        turretAnim.OnShootEvent -= Shoot;
    }

    private void Update()
    {
        if (!hasTarget || isStandby)
            return;

        transform.LookAt(target);
    }

    private void SetState(TurretState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;

        switch (currentState)
        {
            case TurretState.Idle:
                turretAnim.SetShooting(false);
                turretAnim.SetStandby(false);
                turretIndicator.SetIdle();
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0f);
                break;

            case TurretState.Shooting:
                turretAnim.SetShooting(true);
                turretAnim.SetStandby(false);
                turretIndicator.SetShooting();
                break;

            case TurretState.Standby:
                turretAnim.SetShooting(false);
                turretAnim.SetStandby(true);
                turretIndicator.SetStandby();
                break;
        }
    }

    private void TargetDetected()
    {
        hasTarget = true;

        turretSFX.PlayDetection();

        if (!isStandby)
            SetState(TurretState.Shooting);
    }

    private void TargetLost()
    {
        hasTarget = false;
        SetState(TurretState.Idle);
    }

    private void EnterStandby()
    {
        isStandby = true;

        turretSFX.PlayStandby();

        SetState(TurretState.Standby);
    }

    private void ExitStandby()
    {
        isStandby = false;
        turretSFX.StopStandby();

        if (hasTarget)
        {
            SetState(TurretState.Shooting);
            turretSFX.PlayDetection();
        }
        else
            SetState(TurretState.Idle);
    }

    private void Shoot()
    {
        if (!hasTarget || isStandby)
            return;

        turretShooting.Shoot();
        turretSFX.PlayShoot();
        turretVFX.PlayShoot();
        turretSpottedIndicator.TurretShooted();
    }

    protected virtual void HandleDeath()
    {
        turretHealth.OnDestroyed -= HandleDeath;

        if (turretData.turretExplosion != null)
        {
            GameObject explosion = Instantiate(turretData.turretExplosion, transform.position, Quaternion.identity);

            foreach (LootDropper lootDropper in lootDroppers)
            {
                if (lootDropper != null)
                    lootDropper.TryDrop();
            }

            Destroy(explosion, 2f);
        }

        PlayerStatus.Player.AddSuperEnergy(turretData.value);

        if (disableOnDeath)
            gameObject.SetActive(false);
        else
        {
            turretDetection.enabled = false;
            turretShooting.enabled = false;
            Destroy(gameObject);
        }
    }

    private void SpawnDamagePopup(int damage)
    {
        GameObject popup = Instantiate(damagePopupPrefab, damagePopupPoint.position, Quaternion.identity);
        popup.GetComponent<DamagePopup>().Init(damage, target);
    }
}
