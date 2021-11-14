using FluentAssertions;
using System;
using Xunit;

namespace Navigator.Tests
{
    public class NavigationExtensionsTests
    {
        [Fact]
        public void Select_ValidNavigation_CallsSelector()
        {
            var foo = new Foo();

            var root = NavigationFactory.Create(foo);
            var path = root.For(f => f.Bar.Tet.Kip);

            var selectResult = path.Select(kip => $"Selected: {kip}", () => "Not selected");
            selectResult.Should().Be("Selected: Kip!");

            selectResult = path.Select(kip => $"Selected: {kip}");
            selectResult.Should().Be("Selected: Kip!");
        }

        [Fact]
        public void Select_InvalidNavigation_CallsOrGet()
        {
            var foo = new Foo
            {
                Bar = default
            };

            var root = NavigationFactory.Create(foo);
            var path = root.For(f => f.Bar.Tet.Kip);

            var selectResult = path.Select(kip => $"Selected: {kip}", () => "Not selected");
            selectResult.Should().Be("Not selected");

            selectResult = path.Select(kip => $"Selected: {kip}");
            selectResult.Should().Be(default);
        }

        [Fact]
        public void Consume_ValidNavigation_CallsThenCallback()
        {
            var foo = new Foo();

            var root = NavigationFactory.Create(foo);
            var path = root.For(f => f.Bar.Tet.Kip);

            path.Invoking(p => p.Consume(kip =>
            {
                throw new Exception("Then called");
            }, () =>
            {
                throw new Exception("OrElse called");
            })).Should().ThrowExactly<Exception>().WithMessage("Then called");

            path.Invoking(p => p.Consume(kip =>
            {
                throw new Exception("Then called");
            })).Should().ThrowExactly<Exception>().WithMessage("Then called");
        }

        [Fact]
        public void Consume_InvalidNavigation_CallsOrElseCallback()
        {
            var foo = new Foo
            {
                Bar = default
            };

            var root = NavigationFactory.Create(foo);
            var path = root.For(f => f.Bar.Tet.Kip);

            path.Invoking(p => p.Consume(kip =>
            {
                throw new Exception("Then called");
            }, () =>
            {
                throw new Exception("OrElse called");
            })).Should().ThrowExactly<Exception>().WithMessage("OrElse called");

            path.Invoking(p => p.Consume(kip =>
            {
                throw new Exception("Then called");
            })).Should().NotThrow();
        }

        [Fact]
        public void Is_ValidNavigation_CallsPredicate()
        {
            var foo = new Foo();

            var root = NavigationFactory.Create(foo);
            var path = root.For(f => f.Bar.Tet.Kip);

            path.Invoking(p => p.Is(k => throw new Exception("Predicate called")))
                .Should().ThrowExactly<Exception>().WithMessage("Predicate called");
            path.Is(k => k == "Kip!").Should().BeTrue();
            path.Is(k => k != "Kip!").Should().BeFalse();
            path.IsValid().Should().BeTrue();
        }

        [Fact]
        public void Is_InvalidNavigation_DoesNotCallPredicate()
        {
            var foo = new Foo()
            {
                Bar = default
            };

            var root = NavigationFactory.Create(foo);
            var path = root.For(f => f.Bar.Tet.Kip);

            path.Is(k => throw new Exception()).Should().BeFalse();
            path.IsValid().Should().BeFalse();
        }

        private class Foo
        {
            public Bar Bar { get; set; } = new Bar();
        }

        private class Bar
        {
            public Tet Tet { get; set; } = new Tet();
        }

        private class Tet
        {
            public string Kip { get; set; } = "Kip!";
        }
    }
}
