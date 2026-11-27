using UnityEngine;

[CreateAssetMenu(fileName = "SwordStatusSO", menuName = "Scriptable Objects/SwordStatusSO")]
public class SwordStatusSO : ScriptableObject
{
    [Header("Name")]
    public string swordName;

    [Header("Damage")]
    public int minDamage;
    public int maxDamage;
    public int abilityDamage;

    [Header("Range")]
    public int basicRange;
    public int abilityRange;

    [Header("Ability")]
    public int abilityTargets;
    public int abilityCooldown;
    public int superEnergyCost;
}
