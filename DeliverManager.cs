using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeliverManager : MonoBehaviour
{
    public GameObject DroppedPackagePrefab;
    public GameObject MapAddressPosition;
    public Dictionary<Package, bool> packageDeliveredCorrectly;
    // public PackageDb MyTrunkPackages;
    public GameObject Player;
    public GameObject MyTrunkPanel;
    public List<GameObject> DroppedPackages;
    public GameObject PackageLayerGameObject;
    public Vector3 AddressPackageDrop;
    public Package PackageHeld;

    void Start()
    {
        packageDeliveredCorrectly = MyTrunkPanel.GetComponent<MyTrunkPanelManager>().packageDeliveredCorrectly;
    }

    public void DropPackage()
    {
        Debug.Log("Package dropped here");
        PackageHeld = MyTrunkPanel.GetComponent<MyTrunkPanelManager>().PackageHeld;

        AddressPackageDrop = Player.GetComponent<PlayerCollisionManager>().BuildingColliders[0].transform.position;
        AddressPackageDrop.y -= 2.5f;
        Debug.Log(AddressPackageDrop.ToString());
        
        GameObject droppedPackage = Instantiate(DroppedPackagePrefab, AddressPackageDrop, Quaternion.identity, PackageLayerGameObject.transform);
        droppedPackage.name = PackageHeld.packageID;
        droppedPackage.tag = "Package";
        DroppedPackages.Add(droppedPackage);
        Debug.Log("The packages dropped are: " + droppedPackage.ToString());
        checkPackageAddress(this.PackageHeld);
        MyTrunkPanel.GetComponent<MyTrunkPanelManager>().myTrunkPackage.ListOfPackages.Remove(PackageHeld);
        MyTrunkPanel.GetComponent<MyTrunkPanelManager>().SetActiveMyPackageUI();
        //and refresh the mytrunkpanelUI as well.
        //make packageheld = null
    }


    public void PickUpPackage(Package packagePicked)
    {
        GameObject package_picked = DroppedPackages.FirstOrDefault(i => i.name == packagePicked.packageID);
        if(DroppedPackages.Contains(package_picked) && !packageDeliveredCorrectly[packagePicked])
        {
            DroppedPackages.Remove(package_picked);
            MyTrunkPanel.GetComponent<MyTrunkPanelManager>().myTrunkPackage.ListOfPackages.Add(packagePicked);
            MyTrunkPanel.GetComponent<MyTrunkPanelManager>().SetActiveMyPackageUI();
            MyTrunkPanel.GetComponent<MyTrunkPanelManager>().PackageHeld = null;
        }
        else{
            Debug.Log("There may be an error while picking up package: DeliverManager");
        }
    }

    public void checkPackageAddress(Package packageHeld)
    {
        Debug.Log("package id of held package: " + packageHeld.packageID);
        //if the position where the package is dropped is equal to the position of the address indicated on the package,
        // then add this package to the dictionary with the value True. if not, add this package to the dictionary with the value false.
        if(Player.GetComponent<PlayerCollisionManager>().BuildingColliders[0].transform.position == MyTrunkPanel.GetComponent<MyTrunkPanelManager>().PackageHeld.addressRecipient.location)
        {
            Debug.Log("package is dropped at the correct location");
            packageDeliveredCorrectly.Add(packageHeld, true);
        }
        else{
            Debug.Log("package is dropped at the wrong location");
            packageDeliveredCorrectly.Add(packageHeld, false);
        }
    }


}
