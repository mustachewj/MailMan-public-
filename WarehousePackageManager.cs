using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.CompilerServices;
using System;
using UnityEngine.UIElements;

// using Random = System.Random;

public class WarehousePackageManager : MonoBehaviour, IDataPersistence
{
    public static readonly string[] packageIdList = {
        "ZA1234", "YB5678", "XC9012", "WD3456", "VE7890", "UF21s04", "TG6537", "SF9870", "RE3258", "QD7590", "PC1872", "OB6304", "NA9735", "MZ3846", "LY7980", "KX2156", "JW6489", "IV0732", "HU5065", "GT9298", "FS3521", "ER7854", "DQ2187", "CP6510", "BO0843", "AN5176", "ZM9409", "YL3742", "XK8075", "WJ2408", "VI6741", "UH1074", "TG5407", "SF9740", "RE4073", "QD8406", "PC2739", "OB7072", "NA1405", "MZ5738", "LY0071", "KW9314", "JV3647", "IU7980", "HT2313", "GS6645", "FR0978", "EQ5311", "DP9644", "CO3977", "BN8310", "AM2643", "ZL6976", "YK1309", "XJ5642", "WI9975", "VH4308", "UG8641", "TF2974", "SE7307", "RD1640", "QC5973", "PB0306", "OA4639", "NZ8972", "MY3305", "LX7638", "KW1971", "JV6304", "IU0657", "HT5000", "GS9353", "FR3706", "EQ7459", "DP1212", "CO4865", "BN8518", "AM2171", "ZL5824", "YK9477", "XJ3130", "WI6783"
    };
    public PackageDb myTrunkPackages;
    public PackageDb warehousePackages;
    public AddressDb addressDatabase;
    public AddressDb LocalAddressDatabase;
    public Sprite[] spriteList;

    public GameObject myPackagePanel;
    public GameObject packageinfoPanel;
    public List<GameObject> inactivePackagePool;

    public ItemDb UnlockedItemDb;
    public SpecialAbilitiesDb UnlockedSpecialAbilitiesDb;
    public CollectibleObject NextCollectibleObject;
    public int GameDay;
    public int MaxTrunkAmount {get; private set;}
    // public bool MaxTrunkAmountReached;
    // public TrunkManager trunkManager;

    //buffers :: important! change later
    
    public void Awake(){
        // playerLevel = LevelManager.playerLevel;
        GeneratePackages(GameDay);
        inactivePackagePool = new List<GameObject>();
    }

    public void GeneratePackages(int playerLevel){
        System.Random rand = new System.Random();

        if(warehousePackages == null){
            warehousePackages = ScriptableObject.CreateInstance<PackageDb>();
        }
        else{
            warehousePackages.ListOfPackages.Clear();
        }

        if(myTrunkPackages == null){
            myTrunkPackages = ScriptableObject.CreateInstance<PackageDb>();
        }
        else{
            myTrunkPackages.ListOfPackages.Clear();
        }
        

        int[] randomAddressIndexList = new int[addressDatabase.allAddress.Length];
        int[] randomIDIndexList = new int[packageIdList.Length];

        HashSet<int> generatedAddressIndex = new HashSet<int>();
        HashSet<int> generatedIDIndex = new HashSet<int>();

        //until the i reaches max number of addresses, generate a package that will correspond itself to that address
        for (int i = 0; i < addressDatabase.allAddress.Length; i++){
            int randomAddressIndex;
            int randomIDIndex;

            float ItemPackageProbability = CalculateItemPackageProbability(playerLevel);
            float SpecialAbilityPackageProbability = CalculateSpecialAbilityPackageProbability(playerLevel);
            float CollectiblePackageProbability = CalculateCollectiblePackageProbability(playerLevel);

            //randomly generate packageID index and address index in order to mix up the order
            do{
                randomAddressIndex = rand.Next(addressDatabase.allAddress.Length);
            } while (generatedAddressIndex.Contains(randomAddressIndex));
            generatedAddressIndex.Add(randomAddressIndex);

            do{
                randomIDIndex = rand.Next(packageIdList.Length);
            } while (generatedIDIndex.Contains(randomIDIndex));
            generatedIDIndex.Add(randomIDIndex);

            //add the randomly generated IDs into a list
            randomAddressIndexList[i] = randomAddressIndex;
            randomIDIndexList[i] = randomIDIndex;

            // Debug.Log(randomAddressIndex + " and " + randomIDIndex);

            //generate packages of different types depending on the previously-set probability
            int randomPackageTypeThreshold = rand.Next(1);
            if(randomPackageTypeThreshold < ItemPackageProbability){

                //generate an ItemPackage with an item as a reward
                int ItemIndex = rand.Next(UnlockedItemDb.allItems.Count);
                // List<ItemObject> itemObjectList;

                // //fix here!!
                // if(itemObjectList == null){
                //     itemObjectList = new List<ItemObject>();
                // }
                // else
                // {
                //     itemObjectList.Clear();
                //     itemObjectList.Add()
                // }

                Package newPackage = new ItemPackage(addressDatabase.allAddress[randomAddressIndex], addressDatabase.allAddress[rand.Next(addressDatabase.allAddress.Length)], packageIdList[randomIDIndex], spriteList[rand.Next(spriteList.Length)], playerLevel, UnlockedItemDb.allItems[ItemIndex]);
                if(!warehousePackages.ListOfPackages.Contains(newPackage))
                {
                    warehousePackages.ListOfPackages.Add(newPackage);
                    // Debug.Log("added to warehouse: " + newPackage.packageID + newPackage.addressRecipient);
                }
            }

            else if(randomPackageTypeThreshold < ItemPackageProbability + SpecialAbilityPackageProbability)
            {
                int SpecialAbilitiesObjectIndex = rand.Next(UnlockedSpecialAbilitiesDb.allSpecialAbilities.Count);
                
                Package newPackage = new SpecialAbilityPackage(addressDatabase.allAddress[randomAddressIndex], addressDatabase.allAddress[rand.Next(addressDatabase.allAddress.Length)], packageIdList[randomIDIndex], spriteList[rand.Next(spriteList.Length)], playerLevel, UnlockedSpecialAbilitiesDb.allSpecialAbilities[SpecialAbilitiesObjectIndex]);
                if(!warehousePackages.ListOfPackages.Contains(newPackage))
                {
                    warehousePackages.ListOfPackages.Add(newPackage);
                    // Debug.Log("added to warehouse: " + newPackage.packageID + newPackage.addressRecipient);
                }
            }
            else if(randomPackageTypeThreshold < ItemPackageProbability + SpecialAbilityPackageProbability + CollectiblePackageProbability)
            {
                // int CollectibleObjectIndex = rand.Next(gameManagerData.UnlockedCollectibleDb.allCollectibles.Length);
                Package newPackage = new CollectiblePackage(LocalAddressDatabase.allAddress[randomAddressIndex], addressDatabase.allAddress[rand.Next(addressDatabase.allAddress.Length)], packageIdList[randomIDIndex], spriteList[rand.Next(spriteList.Length)], playerLevel, NextCollectibleObject);
                if(!warehousePackages.ListOfPackages.Contains(newPackage))
                {
                    warehousePackages.ListOfPackages.Add(newPackage);
                    // Debug.Log("added to warehouse: " + newPackage.packageID + newPackage.addressRecipient);
                }
            }
            else{
                Package newPackage = new Package(addressDatabase.allAddress[randomAddressIndex], addressDatabase.allAddress[rand.Next(addressDatabase.allAddress.Length)], packageIdList[randomIDIndex], spriteList[rand.Next(spriteList.Length)], playerLevel);
                if(!warehousePackages.ListOfPackages.Contains(newPackage))
                {
                    warehousePackages.ListOfPackages.Add(newPackage);
                    // Debug.Log("added to warehouse: " + newPackage.packageID + newPackage.addressRecipient);
                }
            }           
        }

        // Debug.Log(string.Join(", ", randomAddressIndexList));
        // Debug.Log(string.Join(", ", warehousePackages.ListOfPackages[2].packageQuality));
        
        //edit formula here: currently: linear progression with slope of 0.02
        float CalculateItemPackageProbability(int playerLevel)
        {
            return Mathf.Clamp(playerLevel * 0.03f, 0.0f, 1.0f);
        }
        float CalculateSpecialAbilityPackageProbability(int playerLevel)
        {
            return Mathf.Clamp(playerLevel * 0.02f, 0.0f, 1.0f);
        }
        float CalculateCollectiblePackageProbability(int playerLevel)
        {
            return Mathf.Clamp(playerLevel * 0.01f, 0.0f, 1.0f);
        }

        void CalculateMultipleItemRewardProbability(){
                Debug.Log("yea");
            }
    }

    public void MovePackageToMyTrunk(string packageId, GameObject packageUIObject)
    {
        Debug.Log(GameDay.ToString());
        // Debug.Log("i currently have this amount of items in my trunk:" + myTrunkPackages.ListOfPackages.Count.ToString());
        // Debug.Log("my maximum slot amounts to: " + MaxTrunkAmount.ToString());
        // if(myTrunkPackages.ListOfPackages.Count < MaxTrunkAmount)
        // {
            // warehousePackages.Remov packageID)
            Debug.Log("moving package to my trunk: " + packageId);
            
            Package packageToMove = warehousePackages.ListOfPackages.FirstOrDefault(elem => elem.packageID == packageId);
            //a solution from ChatGPT,
            //FirstOrDefault will return the first element that matches the desired ID or 'null' if
            //no matching element is found.
            if(packageToMove != null)
            {
                bool removeObject = false;
                warehousePackages.ListOfPackages.Remove(packageToMove);
                myTrunkPackages.ListOfPackages.Add(packageToMove);
                inactivePackagePool.Add(packageUIObject);
                // CheckMaximumSlotReached();
                myPackagePanel.GetComponent<MyPackagePanelManager>().UpdateMyPackageUI(removeObject, packageToMove);
                myPackagePanel.GetComponent<MyPackagePanelManager>().UpdateNextButtonInteractable(CheckMaximumSlotReached());

                Debug.Log(string.Join(", ", warehousePackages.ListOfPackages));
                Debug.Log(string.Join(", ", myTrunkPackages.ListOfPackages));
                // myTrunkPackages.ListOfPackages.ForEach(p => Debug.Log("my package quality: " + p.packageQuality));
            
            }
            else
            {
                Debug.Log("a very catrastophic error warning while ADDING package from Fon the Develeoper: Function MovePackageToMyTrunk, selected ID does not match any package. This case should not happen!!");
            }
        // }
        // else
        // {
        //     myPackagePanel.GetComponent<MyPackagePanelManager>().AlertMaximumSlotReached();
        // }
    }
    public bool CheckMaximumSlotReached()
    {
        if(myTrunkPackages.ListOfPackages.Count < MaxTrunkAmount)
        {
            return false;
        }   
        else{
            return true;
        }
    }

    public void ReturnPackageBackToWarehouse(string packageId){
        Package packageToMove = myTrunkPackages.ListOfPackages.FirstOrDefault(elem => elem.packageID == packageId);
        
        if(packageToMove != null)
        {
            bool removeObject = true;
            myTrunkPackages.ListOfPackages.Remove(packageToMove);
            warehousePackages.ListOfPackages.Add(packageToMove);
            GameObject UItoActive = inactivePackagePool.Find(obj => obj.name == packageId);
            UItoActive.SetActive(true);
            myPackagePanel.GetComponent<MyPackagePanelManager>().UpdateMyPackageUI(removeObject, packageToMove);

            // Debug.Log(string.Join(", ", warehousePackages.ListOfPackages));
            // Debug.Log(string.Join(", ", myTrunkPackages.ListOfPackages));
            // myTrunkPackages.ListOfPackages.ForEach(p => Debug.Log("my package quality: " + p.packageQuality));
            
        }
        else
        {
            Debug.Log("a very catrastophic error warning while RETURNING package from Fon the Develeoper: Function MovePackageToMyTrunk, selected ID does not match any package. This case should not happen!!");
        }
    }

    
    
    public void SaveData(ref GameData data)
    {
        data.MyTrunkPackages = myTrunkPackages;
    }

    public void LoadData(GameData data)
    {
        UnlockedItemDb = data.RewardableItemDb;
        UnlockedSpecialAbilitiesDb = data.RewardableSpecialAbilitiesDb;
        GameDay = data.gameDay;
        MaxTrunkAmount = data.MaximumPackageTrunkSlot;

        // var firstFalseEntry = data.CollectibleDb.allCollectibles.FirstOrDefault(entry => !entry.Value);

        // if (firstFalseEntry.Key != null)
        // {
        //     NextCollectibleObject = firstFalseEntry.Key;
        // }
        // else
        // {
        //     Debug.Log("something has gone terribly wrong!");
        // }
    }
}

//     private static WarehousePackageManager instance;
//     void Awake()
//     {
//         if(instance == null)
//         {
//             instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//         PackageIds packageIdListInstance = new PackageIds(packageIdList);
//         console.Log(packageIdListInstance);
//         // GeneratePackage()
//     }

// // pair PackageObject with AddressObject
// // in the future should take in parameters of Levels, Coins, points, days
//     public void GeneratePackage()
//     {
//         for (int i = 0; i < 5; i++) 
//         {
//             //get random package id
//             string id = GetRandomPackageID();

//             // PackageObject newPackage = PackageObject.CreateInstance();
//             // warehousePackages.Add(newPackage);
//             Console.WriteLine(i);
//         }
        
//         console.Log(warehousePackages.listOfPackages);
//     }
//     public static string GetRandomPackageID()
//     {
//         Random random = new Random();
//         int index = rand.Next(packageIdList.Length);
//         return packageIdList[index];
//     }
//     public static PackageObject GetPackageByID(string ID)
//     {
//         foreach (PackageObject package in instance.warehousePackages.listOfPackages)
//         {
//             if (package.packageID == ID)
//             {return package;}
//         }
//         return null;
//     }


