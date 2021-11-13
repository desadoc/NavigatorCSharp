using System;
using System.Collections.Generic;

namespace Navigator
{
    internal class ConditionalCollectionNavigationElement<T>
        : AbstractNavigationElement<IEnumerable<T>, IEnumerable<T>>, ICollectionNavigationElement<T>
        where T : class
    {
        private readonly Func<IEnumerable<T>, bool> predicate;

        public ConditionalCollectionNavigationElement(
            INavigationElement<IEnumerable<T>> parent,
            Func<IEnumerable<T>, bool> predicate)
            : base(parent)
        {
            this.predicate = predicate;
        }

        protected override IEnumerable<T> GetValueFrom(IEnumerable<T> parentValue)
        {
            return predicate(parentValue) ? parentValue : throw new InvalidNavigationException();
        }

        public ICollectionNavigationElement<T> When(Func<IEnumerable<T>, bool> predicate)
        {
            return new ConditionalCollectionNavigationElement<T>(this, predicate);
        }

        public IEnumerator<IObjectNavigationElement<T>> GetEnumerator()
        {
            return new ObjectNavigationElementEnumerator<T>(this, GetValue().GetEnumerator());
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
