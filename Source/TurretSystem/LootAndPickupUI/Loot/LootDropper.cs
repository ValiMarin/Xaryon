using UnityEngine;

public class LootDropper : MonoBehaviour
{
    [Header("Possible Drops")]
    [SerializeField] private GameObject[] itemPrefabs;

    [Header("Drop Chance")]
    [Range(0f, 100f)]
    [SerializeField] private float dropChance = 100f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void TryDrop()
    {
        if (itemPrefabs == null || itemPrefabs.Length == 0)
            return;

        float randomChance = Random.Range(0f, 100f);

        if (randomChance > dropChance)
            return;

        int randomIndex = Random.Range(0, itemPrefabs.Length);

        GameObject selectedPrefab = itemPrefabs[randomIndex];

        Instantiate(selectedPrefab, transform.position, Quaternion.identity);

        audioSource.Play();

        Destroy(this.gameObject, 1f);
    }
}