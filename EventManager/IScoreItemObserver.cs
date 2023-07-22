public interface IScoreItemObserver : IGlobalSubscriber
{
        public void ItemCollected(string nameOfItem);
}
