using System;

public interface IPublisher
{
    public event Action<int> OnItemCountChanged;
    public void Subscribe(Action<int> callback);
    public void Unsubscribe(Action<int> callback);
}