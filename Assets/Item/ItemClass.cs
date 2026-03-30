
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemClass : ScriptableObject
{
    [Header("Item Properties")]
    public string itemName;
    public string itemDescription;
    public Image itemIcon;


    public enum _ItemType
    {
        Misc,      //Sellable, material, ect. [WIP]
        Remedy,    //Heals and buffs that benefit the player
        Inflictor, //Inflicts specific damages and debuffs against opponents
        Rewriter,  //Changes damage type or status effects for the player or opponents
        Chance     //Causes random events
    }
}
