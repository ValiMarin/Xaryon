using UnityEngine;

[CreateAssetMenu(fileName = "PickupSO", menuName = "Scriptable Objects/PickupStatusSO")]
public class PickupSO : ScriptableObject
{
    public string pickupName;

    public GameObject uiPrefab;

    public PickupType pickupType;

    public int amount; 
}
