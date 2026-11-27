using System;
using UnityEngine;

public class TurretAnimationHandler : MonoBehaviour
{
    protected Animator anim;

    [SerializeField] protected TurretAnimationProfileSO profile;

    protected static readonly int ShootingHash = Animator.StringToHash("Shooting");
    protected static readonly int StandByHash = Animator.StringToHash("StandBy");

    public event Action OnShootEvent;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        ApplyProfile();
    }

    protected virtual void ApplyProfile()
    {
        if (profile == null) return;

        anim.SetFloat("IdleSpeed", profile.idleSpeed);
        anim.SetFloat("StandbySpeed", profile.standbySpeed);
        anim.SetFloat("ShootSpeed", profile.shootSpeed);
    }

    public void SetShooting(bool value)
    {
        anim.SetBool(ShootingHash, value);
    }

    public void SetStandby(bool value)
    {
        anim.SetBool(StandByHash, value);
    }

    public void Shoot()
    {
        OnShootEvent?.Invoke();
    }
}