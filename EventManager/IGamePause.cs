public interface IGamePause : IGlobalSubscriber
{
        public void Pause(object sender, bool stop);
}
