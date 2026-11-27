using UnityEngine;

public class SpikesDamageZone : MonoBehaviour
{
    [SerializeField] private int damage = 2;
    [SerializeField] private float tickRate = 1f;

    private PlayerStatus player;
    private int contactCount;

    private float timer;

    private void Update()
    {
        if (contactCount <= 0 || player == null)
        {
            timer = 0f;
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            player.TakeDamage(damage);
            timer = tickRate;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerStatus>(out var p))
            return;

        player = p;
        contactCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<PlayerStatus>(out var p))
            return;

        contactCount = Mathf.Max(0, contactCount - 1);

        if (contactCount == 0)
            player = null;
    }
}