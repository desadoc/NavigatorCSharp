using System;

namespace Navigator.Implementation
{
    internal class ConditionalNavigation<T> : AbstractNavigation<T>
    {
        private readonly INavigation<T> parent;
        private readonly Func<T, bool> predicate;

        public ConditionalNavigation(INavigation<T> parent, Func<T, bool> predicate)
        {
            this.parent = parent;
            this.predicate = predicate;
        }

        public override T GetValue()
        {
            if (parent.TryGetValue(out var parentValue) && predicate(parentValue))
            {
                return parentValue;
            }

            throw new InvalidNavigationException();
        }
    }
}
