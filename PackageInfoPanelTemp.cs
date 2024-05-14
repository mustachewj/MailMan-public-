using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PackageInfoPanelTemp : MonoBehaviour
{
    public TMP_Text Package_id_text;
    public bool PanelActive;
    public MyTrunkPanelManager tmp;
    public GameObject TrunkPanel;
    // public Package PackageToView;
    
    void Start()
    {
        this.gameObject.SetActive(false);
        PanelActive = false;
        tmp = TrunkPanel.GetComponent<MyTrunkPanelManager>();
    }
    public void FillInfoPanel(Package viewing_package)
    {
        // Debug.Log(package_id);
        Debug.Log("friendly click");
        // Package PackageToView = tmp.myTrunkPackage.ListOfPackages.Find(elem => elem.packageID == package_id);
        Package_id_text.text = viewing_package.points.ToString();
    }
}
