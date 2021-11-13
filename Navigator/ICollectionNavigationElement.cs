using System;
using System.Collections.Generic;

namespace Navigator
{
    public interface ICollectionNavigationElement<T>
        : IEnumerable<IObjectNavigationElement<T>>, INavigationElement<IEnumerable<T>>
        where T : class
    {
        ICollectionNavigationElement<T> When(Func<IEnumerable<T>, bool> predicate);
    }
}
