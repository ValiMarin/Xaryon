using UnityEngine;

[CreateAssetMenu(fileName = "TurretAnimationProfileSO", menuName = "Scriptable Objects/TurretAnimationProfileSO")]
public class TurretAnimationProfileSO : ScriptableObject
{
    public float idleSpeed;
    public float standbySpeed;
    public float shootSpeed;
    public float abilitySpeed;
}