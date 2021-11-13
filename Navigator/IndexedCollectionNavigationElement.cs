using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Navigator
{
    internal class IndexedCollectionNavigationElement<T> : IObjectNavigationElement<T>
        where T : class
    {
        private readonly INavigationElement<IEnumerable<T>> parent;
        private readonly int index;

        private T value;

        public IndexedCollectionNavigationElement(INavigationElement<IEnumerable<T>> parent, int index)
        {
            this.parent = parent;
            this.index = index;
        }

        public IObjectNavigationElement<TProperty> For<TProperty>(Expression<Func<T, TProperty>> selector)
            where TProperty : class
        {
            return new NavigationElement<T, TProperty>(this, selector);
        }

        public ICollectionNavigationElement<TProperty> ForEach<TProperty>(
            Expression<Func<T, IEnumerable<TProperty>>> selector)
            where TProperty : class
        {
            return new CollectionNavigationPath<T, TProperty>(this, selector);
        }

        public IObjectNavigationElement<T> When(Func<T, bool> predicate)
        {
            return new ConditionalObjectNavigationElement<T>(this, predicate);
        }

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
            if (this.value != default)
            {
                value = this.value;
                return true;
            }

            if (!parent.TryGetValue(out var parentValue))
            {
                value = default;
                return false;
            }

            try
            {
                this.value = parentValue.ToArray()[index];
                value = this.value;
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                value = default;
                return false;
            }
        }
    }
}
