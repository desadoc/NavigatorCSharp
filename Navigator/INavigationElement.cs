namespace Navigator
{
    public interface INavigationElement<T>
        where T : class
    {
        bool IsValid();
        T GetValue();
        bool TryGetValue(out T value);
    }
}
