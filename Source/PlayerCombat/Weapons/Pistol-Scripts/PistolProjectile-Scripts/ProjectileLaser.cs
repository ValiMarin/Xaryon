using UnityEngine;

public class ProjectileLaser : MonoBehaviour
{
    private LineRenderer line;

    private float range = 1000f;
    
    private LayerMask hitMask;

    private Transform cam;
    private bool active;

    private void Awake()
    {
        cam = GetComponentInParent<Camera>().transform;
        line = GetComponent<LineRenderer>();
        line.enabled = false;

        hitMask = LayerMask.GetMask("Bullets", "Turret-HitBox", "Player");
    }

    public void SetActive(bool value)
    {
        active = value;
        line.enabled = value;
    }

    private void Update()
    {
        if (!active) return;

        Ray rayCam = new Ray(cam.position, cam.forward);
        Vector3 targetPoint = cam.position + cam.forward * range;

        if (Physics.Raycast(rayCam, out RaycastHit hit, range, ~hitMask))
        {
            targetPoint = hit.point;
        }

        Vector3 dir = (targetPoint - transform.position).normalized;
        float dist = Vector3.Distance(transform.position, targetPoint);

        Vector3 endPoint = targetPoint;

        if (Physics.Raycast(transform.position, dir, out RaycastHit obstacleHit, dist, ~hitMask))
        {
            endPoint = obstacleHit.point;
        }

        line.SetPosition(0, transform.position);
        line.SetPosition(1, endPoint);
    }
}