using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Navigator
{
    /// <summary>
    /// This interface represents a navigation to a property starting from a root object.
    /// It allows safe navigation of nested properties when they may be null. It also supports collections
    /// and is able to tell the path to the each property referenced, be it a valid value or null.
    /// </summary>
    /// <typeparam name="T">The type of the property currently navigated to.</typeparam>
    public interface INavigation<T>
    {
        /// <summary>
        /// Creates a child navigation to a property pointed by <paramref name="pathExpression"/>.
        /// </summary>
        /// <example>Examples:
        /// <code>
        ///     var shortNavigation = navigation.For(o => o.Prop);
        ///     var longNavigation = navigation.For(o => o.Prop.AnotherProp);
        /// </code>
        /// </example>
        /// <typeparam name="TProperty">The type of the property <paramref name="pathExpression"/> points to.</typeparam>
        /// <param name="pathExpression">An path expression, see examples.</param>
        /// <returns>A child navigation extending this one.</returns>
        INavigation<TProperty> For<TProperty>(Expression<Func<T, TProperty>> pathExpression);

        /// <summary>
        /// Creates an IEnumerable for a collection given by <paramref name="pathExpression"/>.
        /// In case the property can't be reached, is null or empty, the enumeration will be empty.
        /// </summary>
        /// <example>Example (using System.Linq):
        /// <code>
        ///     var values = navigation.ForEach(o => o.SomeCollection)
        ///         .Select(n => n.GetValue());
        /// </code>
        /// </example>
        /// <typeparam name="TProperty">The type of the values inside the collection</typeparam>
        /// <param name="pathExpression">An path expression, see examples.</param>
        /// <returns>A enumeration of child navigations for a collection property.</returns>
        IEnumerable<INavigation<TProperty>> ForEach<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> pathExpression);

        /// <summary>
        /// Returns a child navigation to the same property of the parent but that's only valid if <paramref name="predicate"/> is true.
        /// </summary>
        /// <param name="predicate">Predicate for the property value.</param>
        /// <returns>A conditionally valid child navigation for the same property.</returns>
        INavigation<T> When(Func<T, bool> predicate);

        /// <summary>
        /// Returns the value of the property target of this navigation.
        /// </summary>
        /// <returns>The value of the property.</returns>
        /// <exception cref="InvalidNavigationException">The property cannot be reached because of a null parent.</exception>
        T GetValue();

        /// <summary>
        /// If avaiable, assigns the value of the property to <paramref name="value"/> and returns true, or false otherwise.
        /// </summary>
        /// <param name="value">Variable to receive the property value.</param>
        /// <returns>A bool that indicates if there was success retrieving the property value.</returns>
        bool TryGetValue(out T value);

        /// <summary>
        /// Returns the path to the current property. It also works for collections while using
        /// <see cref="ForEach{TProperty}(Expression{Func{T, IEnumerable{TProperty}}})"/>, the path will include the index
        /// for the current item.
        /// </summary>
        /// <example>For example:
        /// <code>
        /// var path = navigationRoot.For(r => r.Foo.Bar); // returns "Foo.Bar"
        /// </code>
        /// </example>
        /// <returns></returns>
        string GetPath();
    }
}
