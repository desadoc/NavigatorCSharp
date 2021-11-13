using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Navigator
{
    internal class IndexedCollectionNavigationElement<T>
        : AbstractNavigationElement<IEnumerable<T>, T>, IObjectNavigationElement<T>
        where T : class
    {
        private readonly int index;

        public IndexedCollectionNavigationElement(INavigationElement<IEnumerable<T>> parent, int index)
            : base(parent)
        {
            this.index = index;
        }

        protected override T GetValueFrom(IEnumerable<T> parentValue)
        {
            try
            {
                return parentValue.ToArray()[index];
            }
            catch (IndexOutOfRangeException)
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
