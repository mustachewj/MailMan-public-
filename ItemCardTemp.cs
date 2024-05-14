using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemCardTemp : MonoBehaviour
{
    public TMP_Text ItemNameText;
    public TMP_Text ItemDescriptionText;
    public TMP_Text ItemPriceText;
    public TMP_Text AmountOwnedText;
    public Image ItemImage;
    public GameObject BuyButton;
    public GameObject ShopPanel;

    public void BuyThisItem()
    {
        ShopPanel.GetComponent<ShopManager>().BuyItem(ItemNameText.text);
    }

    public void SetInteractable(bool Interactable)
    {
        BuyButton.GetComponent<Button>().interactable = Interactable;
    }
}
