using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickupUISlot : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject iconRoot;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amountText;

    public PickupType Type { get; private set; }

    public bool IsEmpty => !isActive;
    public bool IsActive => isActive;

    private bool isActive;
    private int amount;
    private float timer;

    private void Awake()
    {
        if (icon == null)
            icon = iconRoot.GetComponent<Image>();

        ResetSlot();
    }

    public void Set(PickupType type, Sprite sprite, float duration)
    {
        Type = type;

        amount = 1;
        timer = duration;
        isActive = true;

        amountText.text = "x1";

        SetIcon(sprite);
    }

    public void Add(float duration)
    {
        if (!isActive) return;

        amount++;
        timer = duration;

        amountText.text = "x" + amount;
    }

    public bool CanStack(PickupType type)
    {
        return isActive && Type == type && timer > 0f;
    }

    public void Tick(float time)
    {
        if (!isActive) return;

        timer -= time;

        if (timer <= 0f)
            ResetSlot();
    }

    private void ResetSlot()
    {
        isActive = false;
        Type = default;
        amount = 0;
        timer = 0;

        amountText.text = "";

        iconRoot.SetActive(false);
    }

    private void SetIcon(Sprite sprite)
    {
        bool hasIcon = sprite != null;

        iconRoot.SetActive(hasIcon);

        if (!hasIcon) return;

        icon.sprite = sprite;
    }
}