namespace Navigator.Implementation
{
    internal class NavigationRoot<T> : AbstractNavigation<T>
    {
        private readonly T subject;

        public NavigationRoot(T subject)
        {
            this.subject = subject;
        }

        public override T GetValue()
        {
            return subject;
        }

        public override string GetPath()
        {
            return string.Empty;
        }
    }
}
