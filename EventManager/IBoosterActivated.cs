using System;

public interface IBoosterActivated : IGlobalSubscriber
{
    public void BoosterActivated(BoosterType boosterType, Action boosterWasActivated);
}
