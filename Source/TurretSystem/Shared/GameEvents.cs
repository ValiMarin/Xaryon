using System;

public static class GameEvents
{
    public static event Action OnHitConfirmed;
    public static event Action<float> OnReticleKick;

    public static void HitConfirmed()
    {
        OnHitConfirmed?.Invoke();
    }

    public static void ReticleKick(float amount)
    {
        OnReticleKick?.Invoke(amount);
    }
}
