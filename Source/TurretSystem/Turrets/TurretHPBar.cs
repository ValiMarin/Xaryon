using UnityEngine;
using UnityEngine.UI;

public class TurretHPBar : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;
    [SerializeField] private Image fillImage;

    private Transform player;

    public void Init(Transform player)
    {
        this.player = player;
    }

    private void OnEnable()
    {
        health.OnHealthChanged += UpdateBar;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= UpdateBar;
    }

    private void UpdateBar(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }

    private void Update()
    {
        if (player != null)
            transform.LookAt(player);
    }
}