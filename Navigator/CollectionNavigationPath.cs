using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Navigator
{
    internal class CollectionNavigationPath<TParent, T>
        : AbstractSelectorNavigationElement<TParent, IEnumerable<T>>, ICollectionNavigationElement<T>
        where TParent : class
        where T : class
    {
        public CollectionNavigationPath(
            INavigationElement<TParent> parent,
            Expression<Func<TParent, IEnumerable<T>>> selector)
            : base(parent, selector)
        {

        }

        public ICollectionNavigationElement<T> When(Func<IEnumerable<T>, bool> predicate)
        {
            return new ConditionalCollectionNavigationElement<T>(this, predicate);
        }

        public IEnumerator<IObjectNavigationElement<T>> GetEnumerator()
        {
            return GetValue()
                .Select((_, index) => new IndexedCollectionNavigationElement<T>(this, index))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
