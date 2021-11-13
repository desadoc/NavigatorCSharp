using System;
using System.Collections.Generic;
using System.Linq;

namespace Navigator
{
    internal class ConditionalCollectionNavigationElement<T> : ICollectionNavigationElement<T>
        where T : class
    {
        private readonly INavigationElement<IEnumerable<T>> parent;
        private readonly Func<IEnumerable<T>, bool> predicate;

        private IEnumerable<T> value;

        public ConditionalCollectionNavigationElement(
            INavigationElement<IEnumerable<T>> parent,
            Func<IEnumerable<T>, bool> predicate)
        {
            this.parent = parent;
            this.predicate = predicate;
        }

        public ICollectionNavigationElement<T> When(Func<IEnumerable<T>, bool> predicate)
        {
            return new ConditionalCollectionNavigationElement<T>(this, predicate);
        }

        public IEnumerable<T> GetValue()
        {
            if (!TryGetValue(out var value))
            {
                throw new InvalidNavigationException();
            }

            return value;
        }

        public bool IsValid()
        {
            return TryGetValue(out var _);
        }

        public bool TryGetValue(out IEnumerable<T> value)
        {
            if (this.value != default)
            {
                if (predicate(this.value))
                {
                    value = this.value;
                    return true;
                }
                else
                {
                    value = default;
                    return false;
                }
            }

            if (!parent.TryGetValue(out var parentValue))
            {
                value = default;
                return false;
            }

            try
            {
                if (predicate(parentValue))
                {
                    value = parentValue;
                    return true;
                }
                else
                {
                    value = default;
                    return false;
                }
            }
            catch (NullReferenceException)
            {
                value = default;
                return false;
            }
        }

        public IEnumerator<IObjectNavigationElement<T>> GetEnumerator()
        {
            return GetValue()
                .Select((_, index) => new IndexedCollectionNavigationElement<T>(this, index))
                .GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
