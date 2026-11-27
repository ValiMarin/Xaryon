using System.Collections.Generic;
using UnityEngine;

public class SwordVFX : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private ParticleSystem attack;

    [Header("Hits VFX")]
    [SerializeField] private GameObject[] hitPrefabs;

    private Dictionary<string, Queue<GameObject>> pool = new();

    private void Awake()
    {
        foreach (GameObject prefab in hitPrefabs)
        {
            pool[prefab.tag] = new Queue<GameObject>();
        }
    }

    public void PlayAttack()
    {
        attack.Play();
    }

    public void PlayHit(RaycastHit hit)
    {
        string tag = hit.transform.tag;

        if (!pool.ContainsKey(tag))
            return;

        GameObject effect = GetFromPool(tag);

        effect.transform.position = hit.point;
        effect.transform.rotation = Quaternion.LookRotation(hit.normal);
        effect.SetActive(true);

        StartCoroutine(ReturnToPool(effect, tag, 1f));
    }

    private GameObject GetFromPool(string tag)
    {
        if (pool[tag].Count > 0)
        {
            return pool[tag].Dequeue();
        }

        GameObject prefab = GetPrefabByTag(tag);
        return Instantiate(prefab);
    }

    private GameObject GetPrefabByTag(string tag)
    {
        foreach (GameObject prefab in hitPrefabs)
        {
            if (prefab.tag == tag)
                return prefab;
        }

        return null;
    }

    private System.Collections.IEnumerator ReturnToPool(GameObject obj, string tag, float delay)
    {
        yield return new WaitForSeconds(delay);

        obj.SetActive(false);
        pool[tag].Enqueue(obj);
    }
}
