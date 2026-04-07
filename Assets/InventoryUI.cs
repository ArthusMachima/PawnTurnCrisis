
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] AudioManager aud;

    [Header("Inventory System")]
    public List<ItemClass> PlayerInventory = new List<ItemClass>();

    [Header("UI Properties")]
    [SerializeField] VerticalLayoutGroup UIGrouper;
    [SerializeField] GameObject Foreground;
    [SerializeField] GameObject ItemDescriptionPanel;
    [SerializeField] GameObject Cylinder;
    [SerializeField] GameObject ItemPanel;
    [SerializeField] ItemPanelClass[] ItemPanelGroup;
    [SerializeField] TextMeshProUGUI DescriptionText;

    bool _isInventoryShown;
    public bool isInventoryShown;
    [SerializeField] bool Controlable=true;
    [SerializeField] int limit;
    [SerializeField] int index;
    [SerializeField] float animTime = 0.5f;
    [SerializeField] float rot = 30;
    [SerializeField] float pos = 30;


    public static InventoryUI Instance;
    private void OnEnable()
    {
        Instance = this;
    }



    private void Start()
    {
        ItemPanelGroup = ItemPanel.GetComponentsInChildren<ItemPanelClass>();
        ShowInventory(false);
    }

    private void Update()
    {
        if (isInventoryShown && Controlable)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(3))
            {
                GameManager.Instance.InventoryMode(false);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                aud.PlaySound(aud.SoundFX, aud.s_CylinderTurn);
                HoverUp();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                aud.PlaySound(aud.SoundFX, aud.s_CylinderTurn);
                HoverDown();
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Z))
            {
                Controlable = false;
                aud.PlaySound(aud.SoundFX, aud.s_CritGunShot);
                SelectItem();
            }
        }

        if (isInventoryShown != _isInventoryShown)
        {
            ShowInventory(isInventoryShown);
            _isInventoryShown = isInventoryShown;
        }
    }
    
    public void DisplayItemPanels()
    {
        limit = -1;
        for (int i = 0; i < ItemPanelGroup.Length; i++)
        {
            if (i < PlayerInventory.Count)
            {
                limit++;
                ItemPanelGroup[i].gameObject.SetActive(true);
                ItemPanelGroup[i].SetItem(PlayerInventory[i]);
            }
            else
            {
                ItemPanelGroup[i].gameObject.SetActive(false);
            }
        }
    }

    

    float UnselectedXPos = 323.0401f;
    float SelectedXPos = 430;

    public void SelectItem()
    {
        if (PlayerInventory.Count != 0)
        {
            StartCoroutine(GameManager.Instance.OnItemUsed(PlayerInventory[index]));
            ItemPanelGroup[index].gameObject.LeanMoveX(4000, 0.2f).setOnComplete(() =>
            {
                ItemPanelGroup[index].transform.LeanMoveX(-3000, 0).setDelay(0.3f);
                PlayerInventory.Remove(PlayerInventory[index]);
            });
        }
        else
        {
            GameManager.Instance.InventoryMode(false);
        }
    }

    float uiShowAnimTime = 0.5f;
    public void ShowInventory(bool shown)
    {
        Controlable = false;
        LeanTween.cancel(Foreground);
        LeanTween.cancel(Cylinder);
        LeanTween.cancel(ItemPanel);
        LeanTween.cancel(ItemDescriptionPanel);

        if (shown)
        {
            DisplayItemPanels();
            index = 0;
            pos = 30;
            SetUIPosition();
            ItemDescriptionPanel.LeanMoveLocalX(-340+(-340+1220.274f), uiShowAnimTime).setEaseInOutQuint();
            Foreground.LeanMoveX(-620 + (-620 + 699.7267f), uiShowAnimTime).setDelay(0.05f).setEaseInOutQuint();
            Cylinder.LeanMoveX(-80+(-80+159.7268f), uiShowAnimTime).setEaseInOutQuint();
            ItemPanel.LeanMoveX(280+(280-200.2732f), uiShowAnimTime).setEaseInOutQuint().setOnComplete(() =>
            {
                Controlable=true;
            });
        }
        else
        {
            ItemDescriptionPanel.LeanMoveLocalX(500+(500+380.2733f), uiShowAnimTime).setEaseInOutQuint();
            Foreground.LeanMoveX(-2000+(-2000+2079.726f), uiShowAnimTime).setEaseInOutQuint();
            Cylinder.LeanMoveX(-400+(-400+159.7268f), uiShowAnimTime).setEaseInOutQuint();
            ItemPanel.LeanMoveX(-850, uiShowAnimTime).setEaseInOutQuint();
        }
        isInventoryShown = shown;
    }

    public void HoverUp()
    {
        if (index>0)
        {
            index--;
            rot -= 60;
            pos -= 83.05995f;
            if (rot < -360) rot = -30;
            SetUIPosition();
        }
    }

    public void HoverDown()
    {
        if (index<limit)
        {
            index++;
            rot += 60;
            pos += 83.05995f;
            if (rot > 360) rot = 30;
            SetUIPosition();
        }
    }

    void SetUIPosition()
    {
        UIGrouper.enabled = false;
        LeanTween.cancel(Cylinder);
        LeanTween.cancel(ItemPanel);
        foreach (ItemPanelClass obj in ItemPanelGroup)
        {
            LeanTween.cancel(obj.gameObject);
            obj.gameObject.LeanMoveLocalX(UnselectedXPos, animTime).setEaseOutQuint();
        }
        LeanTween.cancel(ItemPanelGroup[index].gameObject);
        ItemPanelGroup[index].gameObject.LeanMoveLocalX(SelectedXPos, animTime).setEaseOutQuint();
        ItemPanel.LeanMoveLocalY(pos, animTime).setEaseOutQuint();
        Cylinder.LeanRotate(new(0, 0, rot), animTime).setEaseOutQuint();

        if (PlayerInventory.Count != 0)
            DescriptionText.text = PlayerInventory[index].itemDescription;
        else
            DescriptionText.text = "You have no items.";
    }

}
