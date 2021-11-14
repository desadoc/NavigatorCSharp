using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Navigator.Implementation
{
    internal class CollectionNavigationEnumerable<TParent, T> : IEnumerable<INavigation<T>>
    {
        private readonly INavigation<TParent> parent;
        private readonly Expression<Func<TParent, IEnumerable<T>>> pathExpression;

        public CollectionNavigationEnumerable(
            INavigation<TParent> parent, Expression<Func<TParent, IEnumerable<T>>> pathExpression)
        {
            this.parent = parent;
            this.pathExpression = pathExpression;
        }

        public IEnumerator<INavigation<T>> GetEnumerator()
        {
            var pathNavigation = new PathNavigation<TParent, IEnumerable<T>>(parent, pathExpression);
            if (!pathNavigation.TryGetValue(out var collection) || collection == default)
            {
                return new List<INavigation<T>>().GetEnumerator();
            }

            try
            {
                return collection
                    .Select((item, index) => new CollectionItemNavigation<T>(pathNavigation, item, index))
                    .GetEnumerator();
            }
            catch (NullReferenceException)
            {
                return new List<INavigation<T>>().GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
