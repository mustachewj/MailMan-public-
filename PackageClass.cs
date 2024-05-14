using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Package
{
    // public string packageID { get; protected set; }
    public string packageID; 
    //in between 1-10.0
    public AddressObject addressRecipient { get; protected set; }
    public AddressObject addressSender { get; protected set; }
    public float packageQuality { get; protected set; }
    public Sprite uiDisplay { get; protected set; }
    // public bool rewardMoney;
    public int rewardMoney { get; protected set; }
    public int points { get; protected set; }

    public Package(AddressObject _address_recipient, AddressObject _address_sender, string _packageID, Sprite _uiDisplay, int _playerLevel){
        addressRecipient = _address_recipient;
        addressSender = _address_sender;
        packageID = _packageID;
        uiDisplay = _uiDisplay;
        packageQuality = GeneratePackageQuality(_playerLevel);
        points = GeneratePoint(_playerLevel, _address_recipient);
        rewardMoney = GenerateMoneyAmount(_playerLevel);
        // GenerateReward(this.rewardMoney, this.rewardMoneyAmount, this.rewardItem);
    }

    // private void GenerateReward(bool _rewardMoney, int _rewardMoneyAmount, string _rewarditem){
    //     System.Random rand = new System.Random();
    //     _rewardMoney = rand.Next(2) == 1;
    //     //50% chance of getting money and 50% chance of getting reward item

    //     if (_rewardMoney == true){
    //         _rewardMoneyAmount = rand.Next(500);
    //         _rewarditem = "none";
    //     } else 
    //     {
    //         _rewardMoneyAmount = 0;
    //         _rewarditem = null;
    //     }
    // }
    private int GenerateMoneyAmount(int playerlevel)
    {
        System.Random rand = new System.Random();
        int generatedValue = rand.Next(10, 100);
        return(generatedValue);
    }

    private float GeneratePackageQuality(int playerlevel){
        
        int minQuality;
        int midThresholdQuality;
        int maxQuality = 100;
        int bufferPercentage;
        float generatedQuality;
        
        System.Random rand = new System.Random();
        int num = rand.Next(100);

        
        if(playerlevel <= 50 || playerlevel >= 1){
            bufferPercentage = 25;
            minQuality = 60;
            midThresholdQuality = 80;   
        }
        
        else if(playerlevel <= 100 || playerlevel > 50){
            bufferPercentage = 30;
            minQuality = 60;
            midThresholdQuality = 80;
        }
        
        else if(playerlevel <= 150 || playerlevel > 100){
            bufferPercentage = 35;
            minQuality = 60;
            midThresholdQuality = 80; 
        }
        else 
        //if(playerlevel > 150)
        {
            bufferPercentage = 40;
            minQuality = 60;
            midThresholdQuality = 80; 
        }

        if(num < bufferPercentage){
            num = rand.Next(minQuality,midThresholdQuality+1);
        }
        else
        {
            num = rand.Next(midThresholdQuality,maxQuality+1);
        }

        return generatedQuality = num/10;
    }

    private int GeneratePoint(int playerlevel, AddressObject _address){
        
        int distanceBuffer;
        
        System.Random rand = new System.Random();

        int startingPoint = 500;
        int increasingEarningRate = 35;
         

        if(_address.postNumber < 40300){
            distanceBuffer = rand.Next(50, 100)*-1;
        }
        else if(_address.postNumber < 40500){
            distanceBuffer = rand.Next(-50, 0);
        }
        else if(_address.postNumber < 40600){
            distanceBuffer = rand.Next(0, 50);
        }
        else {
            distanceBuffer = rand.Next(50, 100);
        }

        return startingPoint+(increasingEarningRate*playerlevel)+distanceBuffer;
    }

    public void DecreasePackageQuality(){
        Debug.Log("old points: " + points + "and old package Quality: " + packageQuality);
        packageQuality --;
        points -= 10;
        Debug.Log("new points: " + points + "and new package Quality: " + packageQuality);

    }

    public virtual void ApplyReward(Player player)
    {
        player.AddMoney(rewardMoney);
        player.AddPoints(points);
    }
}




public class ItemPackage : Package
{
    public ItemObject rewardItems { get; protected set; }

    public ItemPackage(AddressObject _address_recipient, AddressObject _address_sender, string _packageID, Sprite _uiDisplay, int _playerLevel, ItemObject reward_items) : base (_address_recipient, _address_sender, _packageID, _uiDisplay, _playerLevel)
    {
        rewardItems = reward_items;
    }

    public override void ApplyReward(Player player)
    {
        base.ApplyReward(player);
        player.AddSpecialItems(rewardItems);
    }

}

public class SpecialAbilityPackage : Package
{
    public SpecialAbilitiesObject SpecialAbility { get; private set; }

    public SpecialAbilityPackage(AddressObject _address_recipient, AddressObject _address_sender, string _packageID, Sprite _uiDisplay, int _playerLevel, SpecialAbilitiesObject special_ability) : base (_address_recipient, _address_sender, _packageID, _uiDisplay, _playerLevel)
    {
        SpecialAbility = special_ability;
    }

    public override void ApplyReward(Player player)
    {
        base.ApplyReward(player);
        player.ActivateSpecialAbility(SpecialAbility);
    }
}


public class CollectiblePackage : Package
{
    public CollectibleObject CollectibleReward { get; private set; }

    public CollectiblePackage(AddressObject _address_recipient, AddressObject _address_sender, string _packageID, Sprite _uiDisplay, int _playerLevel, CollectibleObject _collectibleReward) : base (_address_recipient, _address_sender, _packageID, _uiDisplay, _playerLevel)
    {
        CollectibleReward = _collectibleReward;
    }

    public override void ApplyReward(Player player)
    {
        base.ApplyReward(player);
        player.AddCollectibles(CollectibleReward);
    }
}
