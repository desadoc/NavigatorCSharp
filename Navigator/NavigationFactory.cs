namespace Navigator
{
    public class NavigationFactory
    {
        public static INavigation<T> Create<T>(T value)
        {
            return new Implementation.NavigationRoot<T>(value);
        }
    }
}
