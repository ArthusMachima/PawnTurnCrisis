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

    [Header("Temporary Effects")]
    public int TempHP;
    public int TempATK;
    public int TempElemATK;
    public int TempCritRate;
    public int TempDEF;
    public int TempSPEED;
}