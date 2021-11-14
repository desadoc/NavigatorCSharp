using System;

namespace Navigator
{
    public static class NavigationExtensions
    {
        public static TResult Select<T, TResult>(
            this INavigation<T> navigation,
            Func<T, TResult> selector,
            Func<TResult> orGet)
        {
            return navigation.TryGetValue(out var value) ? selector(value) : orGet();
        }

        public static TResult Select<T, TResult>(
            this INavigation<T> navigation,
            Func<T, TResult> selector)
        {
            return navigation.Select(selector, () => default);
        }

        public static void Consume<T>(this INavigation<T> navigation, Action<T> then, Action orElse)
        {
            if (navigation.TryGetValue(out var value))
            {
                then(value);
                return;
            }

            orElse();
        }

        public static void Consume<T>(this INavigation<T> navigation, Action<T> then)
        {
            navigation.Consume(then, () => { });
        }

        public static bool Is<T>(this INavigation<T> navigation, Func<T, bool> predicate)
        {
            return navigation.TryGetValue(out var value) ? predicate(value) : false;
        }

        public static bool IsValid<T>(this INavigation<T> navigation)
        {
            return navigation.Is(_ => true);
        }
    }
}
