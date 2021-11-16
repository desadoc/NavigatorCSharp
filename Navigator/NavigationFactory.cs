using Navigator.Implementation;

namespace Navigator
{
    public class NavigationFactory
    {
        /// <summary>
        /// Creates a new navigation root for the object passed as parameter.
        /// </summary>
        /// <typeparam name="T">Type of subject for the navigation root</typeparam>
        /// <param name="subject">Instance to navigate</param>
        /// <returns>A navigation root</returns>
        public static INavigation<T> Create<T>(T subject)
        {
            return new NavigationRoot<T>(subject);
        }
    }
}
