using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private PickupSO pickupData;

    private Rigidbody rb;

    private PlayerAmmo playerAmmo;

    private float startForce = 2f;
    private float rotateSpeed = 90f;
    private float moveSpeed = 5f;
    private float acceleration = 15f;
    private float groundCheckDistance = 0.4f;

    [SerializeField] private GameObject audioHolder;

    private Transform playerTarget;

    private bool isGrounded;
    private bool moveToPlayer;

    private float currentSpeed;

    private LayerMask pickupLayerMask;

    private void Start()
    {
        playerTarget = GameObject.FindWithTag("PickupTarget").transform;

        currentSpeed = moveSpeed;

        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * startForce, ForceMode.Impulse);

        pickupLayerMask = LayerMask.GetMask("Bullets", "AimAssist", "Turrent", "PistolProjectile");

        Destroy(gameObject, 100f);
    }

    private void Update()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.IsPaused)
            return;

        Rotate();

        GroundCheck();

        MoveTowardsPlayer();
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.unscaledDeltaTime);
    }

    void GroundCheck()
    {
        if (isGrounded)
            return;

        if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance))
        {
            rb.isKinematic = true;
            isGrounded = true;
        }
    }

    void MoveTowardsPlayer()
    {
        if (!moveToPlayer)
            return;

        transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, currentSpeed * Time.unscaledDeltaTime);

        currentSpeed += acceleration * Time.unscaledDeltaTime;

        if (Vector3.Distance(transform.position, playerTarget.position) < 0.1f)
        {
            Collect();
        }
    }

    void Collect()
    {
        GiveReward();

        PickupUIManager.Instance.ShowPickup(pickupData.pickupType);

        if (audioHolder != null)
        {
            audioHolder.SetActive(true);
            audioHolder.transform.parent = null;

            Destroy(audioHolder, 3f);
        }

        Destroy(this.gameObject);
    }

    void GiveReward()
    {
        switch (pickupData.pickupType)
        {
            case PickupType.MeleeCoin:
                break;

            case PickupType.PistolCoin:
                break;

            case PickupType.ShieldCoin:
                break;

            case PickupType.PistolAmmo:

                if (playerAmmo == null)
                    playerAmmo = FindFirstObjectByType<PlayerAmmo>();

                playerAmmo.AddAmmo(ShootingMode.Pistol, pickupData.amount);
                break;

            case PickupType.SMGAmmo:
                
                if (playerAmmo == null)
                    playerAmmo = FindFirstObjectByType<PlayerAmmo>();

                playerAmmo.AddAmmo(ShootingMode.SMG, pickupData.amount);
                break;

            case PickupType.ProjectileAmmo:
                
                if (playerAmmo == null)
                    playerAmmo = FindFirstObjectByType<PlayerAmmo>();

                playerAmmo.AddAmmo(ShootingMode.Projectile, pickupData.amount);
                break;

            case PickupType.smallBattery:
                PlayerStatus.Player.Heal(pickupData.amount);
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isGrounded)
            return;

        if (!other.CompareTag("Player"))
            return;

        Vector3 direction = playerTarget.position - transform.position;
        float distance = direction.magnitude;

        if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, distance, ~pickupLayerMask))
        {
            if (!hit.collider.CompareTag("Player") && !hit.collider.CompareTag("Shield"))
                return;
        }

        if (pickupData.pickupType == PickupType.smallBattery)
        {
            if (PlayerStatus.Player.CurrentHp >= 100)
                return;
        }

        moveToPlayer = true;
    }
}