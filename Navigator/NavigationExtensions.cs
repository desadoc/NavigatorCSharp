using System;

namespace Navigator
{
    public static class NavigationExtensions
    {
        public static TResult Select<T, TResult>(
            this INavigation<T> navigation,
            Func<INavigation<T>, T, TResult> selector,
            Func<TResult> orGet)
        {
            return navigation.TryGetValue(out var value) ? selector(navigation, value) : orGet();
        }

        public static TResult Select<T, TResult>(
            this INavigation<T> navigation,
            Func<INavigation<T>, TResult> selector,
            Func<TResult> orGet)
        {
            return navigation.Select((_, v) => selector(navigation), orGet);
        }

        public static TResult Select<T, TResult>(
            this INavigation<T> navigation,
            Func<INavigation<T>, T, TResult> selector)
        {
            return navigation.Select(selector, () => default);
        }

        public static TResult Select<T, TResult>(
            this INavigation<T> navigation,
            Func<INavigation<T>, TResult> selector)
        {
            return navigation.Select(selector, () => default);
        }

        public static void Consume<T>(this INavigation<T> navigation, Action<INavigation<T>, T> then, Action orElse)
        {
            if (navigation.TryGetValue(out var value))
            {
                then(navigation, value);
                return;
            }

            orElse();
        }

        public static void Consume<T>(this INavigation<T> navigation, Action<INavigation<T>> then, Action orElse)
        {
            navigation.Consume((_, v) => then(navigation), orElse);
        }

        public static void Consume<T>(this INavigation<T> navigation, Action<INavigation<T>, T> then)
        {
            navigation.Consume(then, () => { });
        }

        public static void Consume<T>(this INavigation<T> navigation, Action<INavigation<T>> then)
        {
            navigation.Consume(then, () => { });
        }

        public static bool Is<T>(this INavigation<T> navigation, Func<T, bool> predicate)
        {
            return navigation.TryGetValue(out var value) && predicate(value);
        }

        public static bool IsValid<T>(this INavigation<T> navigation)
        {
            return navigation.Is(_ => true);
        }
    }
}
