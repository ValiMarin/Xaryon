using UnityEngine;

public class TurretHightlight : MonoBehaviour, IHighlightable
{
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Material highlightMaterial;

    private Material[][] originalMaterials;

    private bool highlighted;

    private Transform player;

    [SerializeField] private float maxDistance = 100f;

    private void Awake()
    {
        if (renderers == null || renderers.Length == 0)
        {
            renderers = GetComponentsInChildren<Renderer>();
        }

        CacheOriginalMaterials();
    }

    public void Init(Transform player)
    {
        this.player = player;
    }

    private void CacheOriginalMaterials()
    {
        originalMaterials = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].materials;
        }
    }

    private void Update()
    {
        if (!highlighted)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        float t = Mathf.Clamp01(distance / maxDistance);

        float outlineWidth = Mathf.Lerp(0.25f, 1f, t * 1.5f);

        ApplyOutlineWidth(outlineWidth);
    }

    private void ApplyOutlineWidth(float width)
    {
        foreach (Renderer r in renderers)
        {
            foreach (Material mat in r.materials)
            {
                mat.SetFloat("_Outline", width);
            }
        }
    }

    public void OnHighlight()
    {
        if (highlighted)
            return;

        highlighted = true;

        foreach (Renderer r in renderers)
        {
            Material[] mats = new Material[r.materials.Length];

            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = highlightMaterial;
            }

            r.materials = mats;
        }
    }

    public void OnHighlightLost()
    {
        if (!highlighted)
            return;

        highlighted = false;

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = originalMaterials[i];
        }
    }

    public HighlightType GetHighlightType()
    {
        return HighlightType.Enemy;
    }

    private void OnDestroy()
    {
        highlighted = false;
    }
}
