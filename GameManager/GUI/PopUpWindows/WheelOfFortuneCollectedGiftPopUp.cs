using UnityEngine;

public sealed class WheelOfFortuneCollectedGiftPopUp : LvlWindowPopUp
{
    [SerializeField] private Transform giftParentTransform;

    public void Initialize(Gift giftToShow)
    {
        var gift = Instantiate(giftToShow,this.transform.position,Quaternion.identity,giftParentTransform);
        gift.Initialize(giftToShow.imageOfGift.sprite,giftToShow.ID,giftToShow.count);
        gift.transform.localPosition = Vector3.zero; 
        gift.AddGiftToInventory();
    }
}
