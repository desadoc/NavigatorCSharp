using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Navigator
{
    internal class ConditionalObjectNavigationElement<T> : IObjectNavigationElement<T>
        where T : class
    {
        private readonly INavigationElement<T> parent;
        private readonly Func<T, bool> predicate;

        private T value;

        public ConditionalObjectNavigationElement(
            INavigationElement<T> parent,
            Func<T, bool> predicate)
        {
            this.parent = parent;
            this.predicate = predicate;
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

        public T GetValue()
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

        public bool TryGetValue(out T value)
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
    }
}
