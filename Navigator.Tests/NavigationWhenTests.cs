using FluentAssertions;
using Xunit;

namespace Navigator.Tests
{
    public class NavigationWhenTests
    {
        [Fact]
        public void When_PredicateIsTrue_IsValidIsTrue()
        {
            var root = new Foo();
            var navigation = NavigationFactory.Create(root);
            var path = navigation.For(f => f.Bar.Kip).When(k => k == "Kip!");

            path.TryGetValue(out var _).Should().BeTrue();
            path.GetValue().Should().Be("Kip!");
            path.GetPath().Should().Be("Bar.Kip");
        }

        [Fact]
        public void When_PredicateIsFalse_IsValidIsFalse()
        {
            var root = new Foo();
            var navigation = NavigationFactory.Create(root);
            var path = navigation.For(f => f.Bar.Kip).When(k => k != "Kip!");

            path.TryGetValue(out var _).Should().BeFalse();
            path.Invoking(p => p.GetValue()).Should().ThrowExactly<InvalidNavigationException>();
            path.GetPath().Should().Be("Bar.Kip");
        }

        [Fact]
        public void When_CompositeNavigationAndPredicateIsTrue_IsValidIsTrue()
        {
            var root = new Foo();
            var navigation = NavigationFactory.Create(root);
            var path = navigation
                .For(f => f.Bar)
                .When(b => b != null)
                .For(b => b.Kip)
                .When(k => k == "Kip!");

            path.TryGetValue(out var _).Should().BeTrue();
            path.GetValue().Should().Be("Kip!");
            path.GetPath().Should().Be("Bar.Kip");
        }

        [Fact]
        public void When_CompositeNavigationAndPredicateIsFalse_IsValidIsFalse()
        {
            var root = new Foo();
            var navigation = NavigationFactory.Create(root);
            var path = navigation
                .For(f => f.Bar)
                .When(b => b != null)
                .For(b => b.Kip)
                .When(k => k == "Kop!");

            path.TryGetValue(out var _).Should().BeFalse();
            path.Invoking(p => p.GetValue()).Should().ThrowExactly<InvalidNavigationException>();
            path.GetPath().Should().Be("Bar.Kip");
        }

        public class Foo
        {
            public Bar Bar { get; set; } = new Bar();
        }


        public class Bar
        {
            public string Kip { get; set; } = "Kip!";
        }
    }
}
