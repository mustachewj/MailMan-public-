using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanelManager : MonoBehaviour
{
    public GameObject shopPanel;
    public ShopManager shopManager;
    public GameObject currentTab;

    public GameObject ItemTab, AbilitiesTab, CharacterUpgradeTab, VehicleUpgradeTab;
    public GameObject ItemCardPrefab, AbilitiesCardPrefab, CharacterCardPrefab, VehicleCardPrefab;
    public Dictionary<ItemObject, bool> BuyableItem;
    public Dictionary<ItemObject, int> OwnedItem;
    public Dictionary<SpecialAbilitiesObject, bool> BuyableSpecialAbilities;
    public Dictionary<SpecialAbilitiesObject, int> OwnedSpecialAbilities;


    void Start()
    {
        currentTab = ItemTab;
        currentTab.SetActive(true);
        shopManager = shopPanel.GetComponent<ShopManager>();
        BuyableItem = shopManager.BuyableItem;
        OwnedItem = shopManager.OwnedItem;
        BuyableSpecialAbilities = shopManager.BuyableSpecialAbilities;
        OwnedSpecialAbilities = shopManager.OwnedSpecialAbilities;
        InitializeItemPanel();
    }

    public void InitializeItemPanel()
    {
        foreach(KeyValuePair<ItemObject, bool> item in BuyableItem)
        {
            GameObject ItemCard = Instantiate(ItemCardPrefab, ItemTab.transform);
            ItemCard.GetComponent<ItemCardTemp>().ShopPanel = shopPanel;
            ItemCard.name = item.Key.itemName;
            ItemCard.GetComponent<ItemCardTemp>().ItemNameText.text = ItemCard.name;
            ItemCard.GetComponent<ItemCardTemp>().ItemDescriptionText.text = item.Key.itemDescription;
            ItemCard.GetComponent<ItemCardTemp>().ItemPriceText.text = item.Key.itemPrice.ToString();
            ItemCard.GetComponent<ItemCardTemp>().ItemImage.sprite = item.Key.itemSprite;
            
            //continue here:: error leads to: entry does not exist in the OwnedItem dictionary, we need <Wood, 0>, not NULL value
            ItemCard.GetComponent<ItemCardTemp>().AmountOwnedText.text = OwnedItem[item.Key].ToString();

            
            if(item.Value == false)
            {
                ItemCard.GetComponent<ItemCardTemp>().SetInteractable(false);
            }
            else{
                ItemCard.GetComponent<ItemCardTemp>().SetInteractable(true);
            }
            
        }
    }

    public void SelectTab(GameObject tab)
    {
        if(currentTab != tab)
        {
            tab.SetActive(true);
            currentTab.SetActive(false);
            currentTab = tab;
        }
    }

    public void OpenItemTab()
    {
        SelectTab(ItemTab);
    }

    public void OpenAbilitiesTab()
    {
        SelectTab(AbilitiesTab);
    }

    public void OpenCharacterUpgradeTab()
    {
        SelectTab(CharacterUpgradeTab);
    }

    public void OpenVehicleUpgradeTab()
    {
        SelectTab(VehicleUpgradeTab);
    }

    public void UpdateOwnedItemAmount(ItemObject itemToUpdate)
    {
        ItemTab.transform.Find(itemToUpdate.itemName).GetComponent<ItemCardTemp>().AmountOwnedText.text = OwnedItem[itemToUpdate].ToString();
    }


}
