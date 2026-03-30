
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanelClass : MonoBehaviour
{
    public ItemClass item;
    [SerializeField] TextMeshProUGUI PanelText;
    [SerializeField] Image PanelIcon;

    public void SetItem(ItemClass givenItem)
    {
        item = givenItem;
        PanelText.text = item.itemName;
        if (item.itemIcon != null) PanelIcon = item.itemIcon;
    }
}
