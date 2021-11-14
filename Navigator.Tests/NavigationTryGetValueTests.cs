using FluentAssertions;
using Xunit;

namespace Navigator.Tests
{
    public class NavigationTryGetValueTests
    {
        [Fact]
        public void TryGetValue_ValidNavigation_ReturnsTrueAndValue()
        {
            var root = new Foo();

            var path = NavigationFactory.Create(root)
                .For(f => f.Bar.Tet.Kip);

            var result = path.TryGetValue(out var valueResult);
            result.Should().BeTrue();
            valueResult.Should().Be("Kip!");
        }

        [Fact]
        public void TryGetValue_ValidCompositePath_ReturnsTrueAndValue()
        {
            var root = new Foo();

            var path = NavigationFactory.Create(root)
                .For(f => f.Bar)
                .For(b => b.Tet)
                .For(t => t.Kip);

            var result = path.TryGetValue(out var valueResult);
            result.Should().BeTrue();
            valueResult.Should().Be("Kip!");
        }

        [Fact]
        public void TryGetValue_InvalidNavigation_ReturnsFalseAndDefault()
        {
            var root = new Foo
            {
                Bar = default
            };

            var path = NavigationFactory.Create(root)
                .For(f => f.Bar.Tet.Kip);

            var result = path.TryGetValue(out var valueResult);
            result.Should().BeFalse();
            valueResult.Should().Be(default);
        }

        [Fact]
        public void TryGetValue_InvalidCompositePath_ReturnsFalseAndDefault()
        {
            var root = new Foo
            {
                Bar = new Bar
                {
                    Tet = default
                }
            };

            var path = NavigationFactory.Create(root)
                .For(f => f.Bar)
                .For(b => b.Tet)
                .For(t => t.Kip);

            var result = path.TryGetValue(out var valueResult);
            result.Should().BeFalse();
            valueResult.Should().Be(default);
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
