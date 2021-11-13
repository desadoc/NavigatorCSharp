namespace Navigator
{
    public abstract class AbstractNavigationElement<TParent, T> : INavigationElement<T>
        where TParent : class
        where T : class
    {
        private readonly INavigationElement<TParent> parent;

        protected AbstractNavigationElement(INavigationElement<TParent> parent)
        {
            this.parent = parent;
        }

        protected abstract T GetValueFrom(TParent parentValue);

        public T GetValue()
        {
            if (!TryGetValue(out var value))
            {
                throw new InvalidNavigationException();
            }

            return value;
        }

        public bool IsValid()
        {
            return TryGetValue(out var _);
        }

        public bool TryGetValue(out T value)
        {
            if (!parent.TryGetValue(out var parentValue))
            {
                value = default;
                return false;
            }

            try
            {
                value = GetValueFrom(parentValue);
                return true;
            }
            catch (InvalidNavigationException)
            {
                value = default;
                return false;
            }
        }
    }
}
