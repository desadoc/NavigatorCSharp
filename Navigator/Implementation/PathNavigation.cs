using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Navigator.Implementation
{
    internal class PathNavigation<TParent, T> : AbstractNavigation<T>
    {
        private static readonly Regex pathStringRegex = new(@".+=>.+?\.(.+)");

        private readonly INavigation<TParent> parent;
        private readonly Func<TParent, T> compiledPathExpression;
        private readonly string pathString;

        public PathNavigation(INavigation<TParent> parent, Expression<Func<TParent, T>> pathExpression)
        {
            this.parent = parent;
            compiledPathExpression = pathExpression.Compile();
            pathString = pathStringRegex
                .Match(pathExpression.ToString())
                .Groups.Values.ToArray()[1].ToString();
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

        public override string GetPath()
        {
            var parentPath = parent.GetPath();
            return string.IsNullOrEmpty(parentPath)
                ? pathString
                : string.Join('.', new[] { parentPath, pathString });
        }
    }
}
