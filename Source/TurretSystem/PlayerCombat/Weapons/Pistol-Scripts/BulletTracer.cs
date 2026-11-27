using UnityEngine;
using System.Collections;

public class BulletTracer : MonoBehaviour
{
    private LineRenderer line;

    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifeAfterHit = 0.1f;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    public void Init(Vector3 start, Vector3 end)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateTracer(start, end));
    }

    private IEnumerator AnimateTracer(Vector3 start, Vector3 end)
    {
        line.enabled = true;

        float t = 0f;

        while (t < 1f)
        {
            Vector3 current = Vector3.Lerp(start, end, t);

            line.SetPosition(0, start);
            line.SetPosition(1, current);

            t += Time.unscaledDeltaTime * speed;
            yield return null;
        }


        line.SetPosition(0, start);
        line.SetPosition(1, end);

        yield return new WaitForSeconds(lifeAfterHit);

        Destroy(gameObject);
    }
}