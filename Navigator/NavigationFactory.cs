namespace Navigator
{
    public class NavigationFactory
    {
        public static IObjectNavigationElement<T> Create<T>(T value) where T : class
        {
            return new NavigationRoot<T>(value);
        }
    }
}
