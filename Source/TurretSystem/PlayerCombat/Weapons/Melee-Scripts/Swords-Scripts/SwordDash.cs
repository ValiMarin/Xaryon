using System;
using UnityEngine;

public class SwordDash : MonoBehaviour
{
     private const float DASH_SPEED = 40f;
     private const float STOP_DISTANCE = 3f;

     private int dashRange = 50;

    private LayerMask dashMask;

    public event Action OnDash;

    private DashPanel dashPanel;

    public IDamageable CurrentTarget { get; private set; }

    private Transform cam;
    private Transform player;
    private Vector3 target;
    private bool isDashing;

    private void Awake()
    {
        cam = GetComponentInParent<Camera>().transform;
        player = cam.root;

        dashMask = LayerMask.GetMask("Player", "Bullets");

        dashPanel = FindFirstObjectByType<DashPanel>();

        dashPanel.Init(this);
    }

    public void InitializeDashRange(int dashRange)
    {
        this.dashRange = dashRange;
    }

    public bool TryStartDash()
    {
        if (!TryFindTarget(out Vector3 hitPoint))
            return false;

        target = hitPoint;
        isDashing = true;
        OnDash?.Invoke();

        return true;
    }

    private void Update()
    {
        if (!isDashing) return;

        float distance = Vector3.Distance(player.position, target);

        if (distance <= STOP_DISTANCE)
        {
            StopDash();
            return;
        }

        player.position = Vector3.MoveTowards(player.position, target, distance / 2 * DASH_SPEED * Time.unscaledDeltaTime);
    }

    private void StopDash()
    {
        isDashing = false;
    }

    private bool TryFindTarget(out Vector3 hitPoint)
    {
        Ray ray = new Ray(cam.position, cam.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, dashRange, ~dashMask))
        {
            if (hit.transform.GetComponentInParent<IDamageable>() is IDamageable dmg)
            {
                CurrentTarget = dmg;
                hitPoint = ((MonoBehaviour)dmg).transform.position;
                return true;
            }
        }

        hitPoint = Vector3.zero;
        return false;
    }
}