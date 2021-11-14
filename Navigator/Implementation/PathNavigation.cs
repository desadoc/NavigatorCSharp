using System;
using System.Linq.Expressions;

namespace Navigator.Implementation
{
    internal class PathNavigation<TParent, T> : AbstractNavigation<T>
    {
        private readonly INavigation<TParent> parent;
        private readonly Expression<Func<TParent, T>> pathExpression;
        private readonly Func<TParent, T> compiledPathExpression;

        public PathNavigation(INavigation<TParent> parent, Expression<Func<TParent, T>> pathExpression)
        {
            this.parent = parent;
            this.pathExpression = pathExpression;
            compiledPathExpression = pathExpression.Compile();
        }

        public override T GetValue()
        {
            try
            {
                return compiledPathExpression.Invoke(parent.GetValue());
            }
            catch (NullReferenceException)
            {
                throw new InvalidNavigationException();
            }
        }
    }
}
