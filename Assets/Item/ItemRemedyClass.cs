using UnityEngine;

[CreateAssetMenu(fileName = "New Remedy", menuName = "Items/Remedy")]
public class ItemRemedyClass : ItemClass
{
    public int PerTurnDuration;
    public GameObject particleEffect;

    [Header("Additive Effects")]
    public int AddedHP;
    public int AddedATK;
    public int AddedElemATK;
    public int AddedCritRate;
    public int AddedDEF;
    public int AddedSPEED;
}

