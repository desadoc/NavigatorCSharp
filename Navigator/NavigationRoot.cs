using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Navigator
{
    internal class NavigationRoot<T> : IObjectNavigationElement<T>
        where T : class
    {
        private readonly T value;

        public NavigationRoot(T value)
        {
            this.value = value;
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

        public bool TryGetValue(out T value)
        {
            value = this.value;
            return true;
        }

        public bool IsValid()
        {
            return true;
        }

        public T GetValue()
        {
            return value;
        }
    }
}
