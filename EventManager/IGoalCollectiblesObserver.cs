public interface IGoalCollectiblesObserver : IGlobalSubscriber
{
        public void ItemCollected(string collectibleName, int remainingCount);
}
