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
            pathString = GetPathString(pathExpression);
        }

        private static string GetPathString(Expression<Func<TParent, T>> pathExpression)
        {
            var groups = pathStringRegex
                .Match(pathExpression.ToString())
                .Groups.Values.ToArray();

            if (groups.Length != 2)
            {
                return string.Empty;
            }

            return groups[1].ToString();
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

            if (!string.IsNullOrEmpty(parentPath) && !string.IsNullOrEmpty(pathString))
            {
                return string.Join('.', new[] { parentPath, pathString });
            }

            return $"{parentPath}{pathString}";
        }
    }
}
