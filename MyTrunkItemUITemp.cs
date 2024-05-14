using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MyTrunkItemUITemp : MonoBehaviour
{
    public GameObject TrunkPanel;
    public MyTrunkPanelManager TrunkPanelManager;
    public GameObject DelivManager;
    public Image background;
    public Sprite unavailable_button_sprite;
    public Color originalColor;
    public Image package_sprite;
    public GameObject view_package_button;
    public TMP_Text package_id;
    public TMP_Text package_address;
    public TMP_Text package_quality;
    public Image package_quality_image;
    public TMP_Text reward_coin;
    public TMP_Text reward_points;
    public GameObject select_button;
    public bool selected;

    public void Start()
    {
        selected = false;
        TrunkPanelManager = TrunkPanel.GetComponent<MyTrunkPanelManager>();
    }
    
    public void View()
    {
        TrunkPanelManager.OpenCloseInfoPanel(this.package_id.text);
        Debug.Log(this.package_id.text);
    }

    public void Select()
    {
        // if(!selected)
        // {
            //if something else is already selected (already something is in the selectedlist)
            //then deselect what is on the list and select this object instead
            if(TrunkPanelManager.SelectedPackageListUI.Count == 1)
            {
                if(TrunkPanelManager.SelectedPackageListUI[0] != this.gameObject)
                {
                    //remove whatever is in the list
                    TrunkPanelManager.SelectedPackageListUI[0].GetComponent<MyTrunkItemUITemp>().SetUITemplateDeselected();
                    TrunkPanelManager.SelectedPackageListUI.Clear();
                    //deselect the already selected object
                    TrunkPanelManager.SelectedPackageListUI.Add(this.gameObject);
                    SetUITemplateSelected();
                    //select the new package
                    TrunkPanelManager.SelectPackage(this.package_id.text);
                    //setting the inventory button image to this package's sprite
                    DelivManager.GetComponent<DeliveryUIManager>().SetImageToPackage(this.package_sprite.sprite);
                    DelivManager.GetComponent<DeliveryUIManager>().DelivButton.SetActive(true);
                }
                else{
                    TrunkPanelManager.SelectedPackageListUI.Clear();
                    SetUITemplateDeselected();
                    TrunkPanelManager.SelectPackage("none");
                    DelivManager.GetComponent<DeliveryUIManager>().SetImageToPackage(DelivManager.GetComponent<DeliveryUIManager>().NotSelected);
                    DelivManager.GetComponent<DeliveryUIManager>().DelivButton.SetActive(false);
                }
            }
            //else (if nothing is in the selectedlist (it is null))
            else if(TrunkPanelManager.SelectedPackageListUI.Count == 0)
            {
                TrunkPanelManager.SelectedPackageListUI.Add(this.gameObject);
                SetUITemplateSelected();
                TrunkPanelManager.SelectPackage(this.package_id.text);
                DelivManager.GetComponent<DeliveryUIManager>().SetImageToPackage(this.package_sprite.sprite);
                DelivManager.GetComponent<DeliveryUIManager>().DelivButton.SetActive(true);
            }
            else{
                Debug.Log("something is very confusing here right now");
            }
    }

    public void SetUITemplateSelected()
    {
        this.background.color = Color.Lerp(this.background.color, Color.black, 0.2f);
    }

    public void SetUITemplateDeselected()
    {
        this.background.color = originalColor;
    }

    public void SetUITemplateDelivered()
    {
        this.background.color = Color.Lerp(this.background.color, Color.black, 0.5f);
        this.select_button.GetComponent<Button>().interactable = false;
        this.select_button.GetComponent<Image>().sprite = unavailable_button_sprite;
    }



}
