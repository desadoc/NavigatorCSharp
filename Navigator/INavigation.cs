using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Navigator
{
    public interface INavigation<T>
    {
        INavigation<TProperty> For<TProperty>(Expression<Func<T, TProperty>> pathExpression);
        IEnumerable<INavigation<TProperty>> ForEach<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> pathExpression);

        INavigation<T> When(Func<T, bool> predicate);

        T GetValue();
        bool TryGetValue(out T value);
    }
}
