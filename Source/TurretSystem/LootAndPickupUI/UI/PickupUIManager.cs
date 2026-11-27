using System.Collections.Generic;
using UnityEngine;

public class PickupUIManager : MonoBehaviour
{
    public static PickupUIManager Instance { get; private set; }

    [System.Serializable]
    private class PickupData
    {
        public PickupType type;
        public Sprite icon;
    }

    [SerializeField] private PickupUISlot[] slots;
    [SerializeField] private PickupData[] pickupData;
    [SerializeField] private float popupDuration = 3f;

    private Dictionary<PickupType, Sprite> icons = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (var data in pickupData)
            icons[data.type] = data.icon;
    }

    private void Update()
    {
        float dt = Time.unscaledDeltaTime;

        for (int i = 0; i < slots.Length; i++)
            slots[i].Tick(dt);
    }

    public void ShowPickup(PickupType type)
    {
        Sprite icon = icons.TryGetValue(type, out var s) ? s : null;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].CanStack(type))
            {
                slots[i].Add(popupDuration);
                return;
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty)
            {
                slots[i].Set(type, icon, popupDuration);
                return;
            }
        }

        slots[0].Set(type, icon, popupDuration);
    }
}