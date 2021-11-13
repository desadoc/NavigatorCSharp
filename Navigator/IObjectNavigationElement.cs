using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Navigator
{
    public interface IObjectNavigationElement<T> : INavigationElement<T>
        where T : class
    {
        IObjectNavigationElement<TProperty> For<TProperty>(Expression<Func<T, TProperty>> selector)
            where TProperty : class;
        ICollectionNavigationElement<TProperty> ForEach<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> selector)
            where TProperty : class;

        IObjectNavigationElement<T> When(Func<T, bool> predicate);
    }
}
