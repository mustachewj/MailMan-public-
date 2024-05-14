using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShopManager : MonoBehaviour, IDataPersistence
{
    public Dictionary<ItemObject, bool> BuyableItem;
    public Dictionary<ItemObject, int> OwnedItem;
    public Dictionary<SpecialAbilitiesObject, bool> BuyableSpecialAbilities;
    public Dictionary<SpecialAbilitiesObject, int> OwnedSpecialAbilities;
    public GameObject shopPanel;
    public PlayerStatsManager playerStats;
    
    void Start()
    {
        // shopPanelManager = ShopContent.GetComponent<ShopPanelManager>();
        playerStats = GameObject.Find("PlayerStatsPanel").GetComponent<PlayerStatsManager>();
        Debug.Log("you currently own: " + OwnedItem);
        Debug.Log("you can currently buy: " + BuyableItem);
    }

    public void SaveData(ref GameData data)
    {
        data.OwnedItem = OwnedItem;
        data.BuyableItem = BuyableItem;
        data.OwnedSpecialAbilities = OwnedSpecialAbilities;
        data.BuyableSpecialAbilities = BuyableSpecialAbilities;
    }

    public void LoadData(GameData data)
    {
        BuyableItem = data.BuyableItem;
        OwnedItem = data.OwnedItem;
        BuyableSpecialAbilities = data.BuyableSpecialAbilities;
        OwnedSpecialAbilities = data.OwnedSpecialAbilities;
    }
    
    public void BuyItem(string ItemName)
    {
        Debug.Log("Buying this item");
        
        ItemObject itemToBuy = BuyableItem.FirstOrDefault(kv => kv.Key.itemName == ItemName).Key;

        if(BuyableItem[itemToBuy])
        {
            Debug.Log(ItemName + " is buyable! ...checking for amount of coins");
            if(playerStats.coins >= itemToBuy.itemPrice)
            {
                playerStats.coins = playerStats.coins - itemToBuy.itemPrice;
                // OwnedItem[itemToBuy]++;
                if (OwnedItem.ContainsKey(itemToBuy))
                {
                    // Increment the value associated with the key
                    OwnedItem[itemToBuy]++;
                }
                else
                {
                    // Add the item to the dictionary with an initial count of 1
                    Debug.LogError("system error: item does not exist in game data");
                }
                UpdateOwnedItemAmount(itemToBuy);
                playerStats.UpdateDataUICoins();
                Debug.Log("Purchase successful");
                Debug.Log("You now own: " + OwnedItem);
            }
            else
            {
                Debug.Log("You don't have enough coins, :(");
            }
        }
        else
        {
            Debug.Log(ItemName + " is not available");
        }
        // if(ItemName)
    }

    public void UpdateOwnedItemAmount(ItemObject itemToUpdate)
    {
        shopPanel.GetComponent<ShopPanelManager>().UpdateOwnedItemAmount(itemToUpdate);
    }
}
