using UnityEngine;

[CreateAssetMenu(fileName = "New Inflictor", menuName = "Items/Inflictor")]
public class ItemInflictorClass : ItemClass
{
    public int PerTurnDuration;

    [Header("Additive Effects")]
    public int AddedHP;
    public int AddedATK;
    public int AddedElemATK;
    public int AddedCritRate;
    public int AddedDEF;
    public int AddedSPEED;
}


