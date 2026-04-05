
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemClass : ScriptableObject
{
    [Header("Item Properties")]
    public string itemName;
    public string itemDescription;
    public Image itemIcon;
}

public class TempEffect
{
    public int TempHP;
    public int TempATK;
    public int TempElemATK;
    public int TempCritRate;
    public int TempDEF;
    public int TempSPEED;

    public int PerTurnDuration;
}
