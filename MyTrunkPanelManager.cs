using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTrunkPanelManager : MonoBehaviour, IDataPersistence
{
    public GameObject GameScenePackageInfoPanel;
    // public GameObject MyTrunkPanel;
    public PackageDb myTrunkPackage;
    public Dictionary<Package, bool> packageDeliveredCorrectly;
    public List<GameObject> myPackageUIList;
    public List<Sprite> PackageQualitySprites;
    public Package PackageHeld;
    public List<GameObject> SelectedPackageListUI;
    public bool TrunkOpened;
    // public DeliverManager DelivManager;
    // public GameObject Managers;


    void Start()
    {
        gameObject.SetActive(false);
        GameScenePackageInfoPanel.SetActive(false);
        SetActiveMyPackageUI();
        TrunkOpened = false;
    
    }

    public void SetPackageInfoPanelActive(string packageid)
    {

    }

    public void OpenCloseTrunk()
    {
        if(!TrunkOpened)
        {
            gameObject.SetActive(true);
            TrunkOpened = true;
        }
        else
        {
            gameObject.SetActive(false);    
            TrunkOpened = false;
        }
    }

    public void OpenCloseInfoPanel(string packageid)
    {
        if(!GameScenePackageInfoPanel.GetComponent<PackageInfoPanelTemp>().PanelActive)
        {
            Package viewingPackage = myTrunkPackage.ListOfPackages.Find(elem => elem.packageID == packageid);
            GameScenePackageInfoPanel.GetComponent<PackageInfoPanelTemp>().FillInfoPanel(viewingPackage);
            GameScenePackageInfoPanel.SetActive(true);
            GameScenePackageInfoPanel.GetComponent<PackageInfoPanelTemp>().PanelActive = true;
        }
        else
        {
            GameScenePackageInfoPanel.SetActive(false);    
            GameScenePackageInfoPanel.GetComponent<PackageInfoPanelTemp>().PanelActive = false;
        }
        
    }

    public void RemovePackage()
    {

    }

    public void SetActiveMyPackageUI()
    {
        Color ItemPackageColor = new Color(1.0f, 0.9649609f, 0.7803922f);
        Color SpecialAbilityPackageColor = new Color(1.0f, 0.8742138f, 0.9508058f);
        Color CollectiblePackageColor = new Color(0.7803922f, 0.8855919f, 1.0f);

        for(int i=0; i < myTrunkPackage.ListOfPackages.Count; i++)
        {
            myPackageUIList[i].SetActive(true);
            myPackageUIList[i].GetComponent<MyTrunkItemUITemp>().package_sprite.sprite = myTrunkPackage.ListOfPackages[i].uiDisplay;
            myPackageUIList[i].GetComponent<MyTrunkItemUITemp>().package_id.text = myTrunkPackage.ListOfPackages[i].packageID;
            myPackageUIList[i].GetComponent<MyTrunkItemUITemp>().package_address.text = myTrunkPackage.ListOfPackages[i].addressRecipient.streetName;
            myPackageUIList[i].GetComponent<MyTrunkItemUITemp>().reward_coin.text = myTrunkPackage.ListOfPackages[i].rewardMoney.ToString();
            myPackageUIList[i].GetComponent<MyTrunkItemUITemp>().reward_points.text = myTrunkPackage.ListOfPackages[i].points.ToString();
            myPackageUIList[i].GetComponent<MyTrunkItemUITemp>().package_quality.text = myTrunkPackage.ListOfPackages[i].packageQuality.ToString();

            switch(Math.Ceiling((double)myTrunkPackage.ListOfPackages[i].packageQuality/2))
            {
                case 1:
                    myPackageUIList[i].GetComponent<MyTrunkItemUITemp>().package_quality_image.sprite = PackageQualitySprites[0];
                    break;
                case 2:
                    myPackageUIList[i].GetComponent<MyTrunkItemUITemp>().package_quality_image.sprite = PackageQualitySprites[1];
                    break;
                case 3:
                    myPackageUIList[i].GetComponent<MyTrunkItemUITemp>().package_quality_image.sprite = PackageQualitySprites[2];
                    break;    
                case 4:
                    myPackageUIList[i].GetComponent<MyTrunkItemUITemp>().package_quality_image.sprite = PackageQualitySprites[3];
                    break;
                case 5:
                    myPackageUIList[i].GetComponent<MyTrunkItemUITemp>().package_quality_image.sprite = PackageQualitySprites[4];
                    break; 
                default:
                    Console.WriteLine("Unexpected case in MyTrunkPanelManager");
                    break;   
            }

            if(myTrunkPackage.ListOfPackages[i] is ItemPackage)
            {
                myPackageUIList[i].GetComponent<MyTrunkItemUITemp>().background.color = ItemPackageColor;
                myPackageUIList[i].GetComponent<MyTrunkItemUITemp>().originalColor = ItemPackageColor;
            }
            else if(myTrunkPackage.ListOfPackages[i] is SpecialAbilityPackage)
            {
                myPackageUIList[i].GetComponent<MyTrunkItemUITemp>().background.color = SpecialAbilityPackageColor;
                myPackageUIList[i].GetComponent<MyTrunkItemUITemp>().originalColor = SpecialAbilityPackageColor;
            }
            else if(myTrunkPackage.ListOfPackages[i] is CollectiblePackage)
            {
                myPackageUIList[i].GetComponent<MyTrunkItemUITemp>().background.color = CollectiblePackageColor;
                myPackageUIList[i].GetComponent<MyTrunkItemUITemp>().originalColor = CollectiblePackageColor;
            }
            else{
                Debug.Log("Error, package type is neither Item, Special Ability, not Collectible, error in MyTrunkPanelManager");
            }
        }
    }

    public void UpdateMyPackageQualityUI()
    {

    }

    public void SelectPackage(string package_ID)
    {
        if(package_ID == "none")
        {
            PackageHeld = null;
        }
        else{
            PackageHeld = myTrunkPackage.ListOfPackages.Find(i => i.packageID == package_ID);
        }
    }

    public void DeselectPackage()
    {
        PackageHeld = null;
    }
    

    public void SaveData(ref GameData data)
    {
        data.MyTrunkPackages = myTrunkPackage;
        data.PackagesDeliveredCorrectly = packageDeliveredCorrectly;
    }

    public void LoadData(GameData data)
    {
        myTrunkPackage = data.MyTrunkPackages;
        packageDeliveredCorrectly = data.PackagesDeliveredCorrectly;
    }




}
