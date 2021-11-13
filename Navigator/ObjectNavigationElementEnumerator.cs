using System;
using System.Collections;
using System.Collections.Generic;

namespace Navigator
{
    internal class ObjectNavigationElementEnumerator<T> : IEnumerator<IObjectNavigationElement<T>>, IDisposable
        where T : class
    {
        private readonly INavigationElement<IEnumerable<T>> parent;
        private readonly IEnumerator<T> backingEnumerator;

        private int currentIndex;

        public ObjectNavigationElementEnumerator(
            INavigationElement<IEnumerable<T>> parent, IEnumerator<T> backingEnumerator)
        {
            this.parent = parent;
            this.backingEnumerator = backingEnumerator;
        }

        IObjectNavigationElement<T> IEnumerator<IObjectNavigationElement<T>>.Current =>
            new IndexedCollectionNavigationElement<T>(parent, backingEnumerator.Current, currentIndex);

        object IEnumerator.Current =>
            new IndexedCollectionNavigationElement<T>(parent, backingEnumerator.Current, currentIndex);

        public void Dispose()
        {
            backingEnumerator.Dispose();
        }

        public bool MoveNext()
        {
            if (backingEnumerator.MoveNext())
            {
                currentIndex += 1;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            currentIndex = default;
            backingEnumerator.Reset();
        }
    }
}
