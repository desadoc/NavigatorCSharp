using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Navigator.Implementation
{
    internal abstract class AbstractNavigation<T> : INavigation<T>
    {
        public INavigation<TProperty> For<TProperty>(Expression<Func<T, TProperty>> pathExpression)
        {
            return new PathNavigation<T, TProperty>(this, pathExpression);
        }

        public IEnumerable<INavigation<TProperty>> ForEach<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> pathExpression)
        {
            return new CollectionNavigationEnumerable<T, TProperty>(this, pathExpression);
        }

        public INavigation<T> When(Func<T, bool> predicate)
        {
            return new ConditionalNavigation<T>(this, predicate);
        }

        public abstract T GetValue();

        public bool TryGetValue(out T value)
        {
            try
            {
                value = GetValue();
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
