using System;
using UnityEngine;

public class TurretDetection : MonoBehaviour
{
    private Transform target;

    private float detectionRange;
    private float intimidationRange;

    private LayerMask ignoreMask;

    private bool hasTarget;
    private bool isStandby;

    public event Action OnTargetDetected;
    public event Action OnTargetLost;
    public event Action OnStandbyEnter;
    public event Action OnStandbyExit;

    private float detectationDelay = 1f;
    private float standbyDelay = 0.5f;

    public bool turretSpotted { get; private set; }

    public void Init(Transform target, float detectionRange, float intimidationRange)
    {
        this.target = target;
        this.detectionRange = detectionRange;
        this.intimidationRange = intimidationRange;

        ignoreMask = LayerMask.GetMask("PistolProjectile", "Turret", "Bullets", "DestructibleObject", "AimAssist");
    }

    private void Update()
    {
        Detect();
    }

    private void Detect()
    {
        if (target == null)
        {
            LoseTarget();
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;

        if (!Physics.Raycast(transform.position, dir, out RaycastHit hit, detectionRange, ~ignoreMask))
        {
            LoseTarget();
            return;
        }

        bool isPlayer = hit.transform.CompareTag("Player") || hit.transform.CompareTag("Shield");

        if (!isPlayer)
        {
            LoseTarget();
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        HandleStandby(distance);

        HandleDetection();
    }

    private void HandleStandby(float distance)
    {
        if (distance <= intimidationRange)
        {
            standbyDelay = 0.5f;

            if (isStandby)
                return;

            isStandby = true;
            OnStandbyEnter?.Invoke();
            return;
        }

        if (isStandby)
        {
            if (standbyDelay > 0f)
            {
                standbyDelay -= Time.deltaTime;
                return;
            }

            isStandby = false;
            OnStandbyExit?.Invoke();
        }
    }

    private void HandleDetection()
    {
        if (hasTarget)
            return;

        turretSpotted = false;

        if (detectationDelay > 0f)
        {
            detectationDelay -= Time.deltaTime;
            return;
        }

        hasTarget = true;
        OnTargetDetected?.Invoke();
    }

    private void LoseTarget()
    {
        detectationDelay = 1f;
        turretSpotted = true;

        if (!hasTarget && !isStandby)
            return;

        hasTarget = false;

        if (isStandby)
        {
            isStandby = false;
            OnStandbyExit?.Invoke();
        }

        OnTargetLost?.Invoke();
    }

    public void ResetDetection()
    {
        hasTarget = false;
        isStandby = false;

        detectationDelay = 1f;
        standbyDelay = 0.5f;
    }
}