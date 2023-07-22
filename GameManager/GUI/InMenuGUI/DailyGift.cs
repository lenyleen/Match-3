using System;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class DailyGift : Gift
{
    [SerializeField] private Text dayNumber;
    [SerializeField] private Image setActiveTodayImage;
    private event Action actionOnClick;
    private Button button => this.GetComponent<Button>();

    public void Initialize(Sprite giftSprite, int ID, int count,int dayNumber,bool activeToday, Action actionOnClick)
    {
        base.Initialize(giftSprite, ID, count);
        this.dayNumber.text = dayNumber.ToString();
        this.actionOnClick += actionOnClick;
        if (activeToday)
        {
            setActiveTodayImage.enabled = true;
            button.enabled = true;
            button.onClick.AddListener(AddGiftToInventory);
        }
        
    } 
    public override void AddGiftToInventory()
    {
        setActiveTodayImage.enabled = false;
        base.AddGiftToInventory();
        actionOnClick?.Invoke();
    }
}
