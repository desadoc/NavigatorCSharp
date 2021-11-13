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
        public void TryGetValue_ValidCompositePath_CorrectValue()
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
