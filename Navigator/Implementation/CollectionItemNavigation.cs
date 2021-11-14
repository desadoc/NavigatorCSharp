using System.Collections.Generic;

namespace Navigator.Implementation
{
    internal class CollectionItemNavigation<T> : AbstractNavigation<T>
    {
        private readonly INavigation<IEnumerable<T>> parent;
        private readonly T item;
        private readonly int index;

        public CollectionItemNavigation(INavigation<IEnumerable<T>> parent, T item, int index)
        {
            this.parent = parent;
            this.item = item;
            this.index = index;
        }

        public override T GetValue()
        {
            return item;
        }

        public override string GetPath()
        {
            return $"{parent.GetPath()}[{index}]";
        }
    }
}
