using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameObjects.Candies
{
    public abstract class BonusCandy : Candy
    {
        public Dictionary<Type, Func<Candy,Task>> bonusActions;

        public abstract Task<bool> BonusAction(Candy candy);
        
        public virtual Task SuperBonus()
        {
            boxCollider.enabled = false;
            ReturnCandyToPool();
            return Task.CompletedTask;
        }
        public virtual Task SuperBonus(Candy switchedCandy)
        {
            return Task.CompletedTask;
        }
        
    }
}