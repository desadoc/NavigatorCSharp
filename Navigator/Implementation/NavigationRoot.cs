namespace Navigator.Implementation
{
    internal class NavigationRoot<T> : AbstractNavigation<T>
    {
        private readonly T value;

        public NavigationRoot(T value)
        {
            this.value = value;
        }

        public override T GetValue()
        {
            return value;
        }

        public override string GetPath()
        {
            return string.Empty;
        }
    }
}
