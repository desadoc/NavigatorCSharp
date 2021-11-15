namespace Navigator
{
    public class NavigationFactory
    {
        /// <summary>
        /// Creates a new navigation root for the object passed as parameter.
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="value">Object to navigate</param>
        /// <returns>A navigation root</returns>
        public static INavigation<T> Create<T>(T value)
        {
            return new Implementation.NavigationRoot<T>(value);
        }
    }
}
