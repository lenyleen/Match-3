public class SerializeElements
{
    
}
[System.Serializable]
public struct Element<TElement>
{
    public int Index0;
    public int Index1;
    public TElement _element;
    public Element(int idx0, int idx1, TElement element)
    {
        Index0 = idx0;
        Index1 = idx1;
        _element = element;
    }
}

[System.Serializable]
public struct SerializableElement<TObject>
{
    public int count;
    public TObject _obj;

    public SerializableElement(int count, TObject obj)
    {
        this.count = count;
        _obj = obj;
    }
}