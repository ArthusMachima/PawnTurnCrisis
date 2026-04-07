
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemClass : ScriptableObject
{
    [Header("Item Properties")]
    public string itemName;
    public string itemDescription;
    public Image itemIcon;
    public int RarityValue;
}
