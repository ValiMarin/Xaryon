using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private int maxHP;
    private int currentHP;

    public float CurrentHP => currentHP;
    public float MaxHP => maxHP;

    public event Action<float, float> OnHealthChanged;
    public event Action<int> OnDamageTaken;
    public event Action OnDestroyed;

    public void Init(int maxHP)
    {
        this.maxHP = maxHP;
        currentHP = maxHP;

        OnHealthChanged?.Invoke(currentHP, maxHP);
    }

    public void TakeDamage(int damage)
    {
        if (currentHP <= 0f || damage <= 0)
            return;

        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        OnDamageTaken?.Invoke(damage);
        OnHealthChanged?.Invoke(currentHP, maxHP);

        GameEvents.HitConfirmed();

        if (currentHP <= 0f)
            Die();
    }

    private void Die()
    {
        OnDestroyed?.Invoke();
    }
}