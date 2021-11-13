using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Navigator
{
    internal class ConditionalObjectNavigationElement<T> : AbstractNavigationElement<T, T>, IObjectNavigationElement<T>
        where T : class
    {
        private readonly Func<T, bool> predicate;

        public ConditionalObjectNavigationElement(
            INavigationElement<T> parent,
            Func<T, bool> predicate)
            : base(parent)
        {
            this.predicate = predicate;
        }

        protected override T GetValueFrom(T parentValue)
        {
            return predicate(parentValue) ? parentValue : throw new InvalidNavigationException();
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
