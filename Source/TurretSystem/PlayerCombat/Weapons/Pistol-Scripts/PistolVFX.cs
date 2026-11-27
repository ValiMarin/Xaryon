using UnityEngine;
using System.Collections.Generic;

public class PistolVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem shootPistolMuzzle, shootSMGMuzzle, shootProjectileMuzzle;

    private PistolController pistol;
    private PistolSO data;

    private Dictionary<string, Queue<GameObject>> pool = new();

    private void Awake()
    {
        pistol = GetComponent<PistolController>();
        data = pistol.Data;

        foreach (GameObject prefab in data.hitPrefabs)
        {
            pool[prefab.tag] = new Queue<GameObject>();
        }
    }

    public void PlayPistolMuzzle()
    {
        shootPistolMuzzle.Play();
    }

    public void PlaySMGMuzzle()
    {
        shootSMGMuzzle.Play();
    }

    public void PlayProjectileLauncherMuzzle()
    {
        shootProjectileMuzzle.Play();
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
        foreach (GameObject prefab in data.hitPrefabs)
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
