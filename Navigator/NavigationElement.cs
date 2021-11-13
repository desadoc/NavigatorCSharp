using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Navigator
{
    internal class NavigationElement<TParent, T> : AbstractNavigationElement<TParent, T>, IObjectNavigationElement<T>
        where TParent : class
        where T : class
    {
        private readonly Expression<Func<TParent, T>> selector;

        public NavigationElement(
            INavigationElement<TParent> parent,
            Expression<Func<TParent, T>> selector)
            : base(parent)
        {
            this.selector = selector;
        }

        protected override T GetValueFrom(TParent parentValue)
        {
            try
            {
                return selector.Compile().Invoke(parentValue);
            }
            catch (NullReferenceException)
            {
                throw new InvalidNavigationException();
            }
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
