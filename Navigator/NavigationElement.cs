using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Navigator
{
    internal class NavigationElement<TParent, T> : AbstractSelectorNavigationElement<TParent, T>, IObjectNavigationElement<T>
        where TParent : class
        where T : class
    {
        public NavigationElement(
            INavigationElement<TParent> parent,
            Expression<Func<TParent, T>> selector)
            : base(parent, selector)
        {

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
    }
}
