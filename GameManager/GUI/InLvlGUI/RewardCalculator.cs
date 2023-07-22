using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardCalculator
{
    private List<StoreItem> dataOfItems;

    public RewardCalculator(List<StoreItem> dataOfItems)
    {
        this.dataOfItems = dataOfItems.OrderBy(item => item.dropChance).ToList();
    }

    public List<StoreItem> CalculateDrop(params int[] dropChance)
    {
        var reward = new List<StoreItem>();
        for (int i = 0; i < dropChance.Length; i++)
        {
            int calc_chance = Random.Range(0,100);
            if(calc_chance > dropChance[i])
            {
                return reward;
            }
            if(calc_chance <= dropChance[i])
            {
                int randomValue = Random.Range(1, 100);
                for (int k = 0; k < dataOfItems.Count ;k++)
                {
                    if(randomValue <= dataOfItems[k].dropChance)
                    {
                        reward.Add(dataOfItems[k]);
                        break;
                    }
                }
            }
        }
        return reward;
    }
}
