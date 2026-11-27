using UnityEngine;

public class TurretIndicator : MonoBehaviour
{
    [Header("Renderer")]
    [SerializeField] private Renderer indicatorRenderer;

    [Header("Materials")]
    [SerializeField] private Material idleMaterial;
    [SerializeField] private Material shootingMaterial;
    [SerializeField] private Material standbyMaterial;

    [Header("Particles")]
    [SerializeField] private GameObject green;
    [SerializeField] private GameObject red;
    [SerializeField] private GameObject yellow;

    public void SetIdle()
    {
        indicatorRenderer.material = idleMaterial;
        green.SetActive(true);
        red.SetActive(false);
        yellow.SetActive(false);
    }

    public void SetShooting()
    {
        indicatorRenderer.material = shootingMaterial;
        green.SetActive(false);
        red.SetActive(true);
        yellow.SetActive(false);
    }

    public void SetStandby()
    {
        indicatorRenderer.material = standbyMaterial;
        green.SetActive(false);
        red.SetActive(false);
        yellow.SetActive(true);
    }
}