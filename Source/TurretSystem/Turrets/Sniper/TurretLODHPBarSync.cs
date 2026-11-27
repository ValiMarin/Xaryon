using UnityEngine;

public class TurretLODHPBarSync : MonoBehaviour
{
    [SerializeField] private LODGroup lodGroup;
    [SerializeField] private GameObject hpBar;

    private Renderer[] renderers;

    private void Awake()
    {
        renderers = lodGroup.GetComponentsInChildren<Renderer>();
    }

    private void LateUpdate()
    {
        bool visible = false;

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null && renderers[i].isVisible)
            {
                visible = true;
                break;
            }
        }

        hpBar.SetActive(visible);
    }
}