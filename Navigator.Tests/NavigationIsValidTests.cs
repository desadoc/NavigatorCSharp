using FluentAssertions;
using Xunit;

namespace Navigator.Tests
{
    public class NavigationIsValidTests
    {
        [Fact]
        public void IsValid_ValidNavigation_ReturnsTrue()
        {
            var root = new Foo();
            var path = NavigationFactory.Create(root)
                .For(f => f.Bar.Tet.Kip);

            path.IsValid().Should().BeTrue();
        }

        [Fact]
        public void IsValid_ValidCompositePath_CorrectValue()
        {
            var root = new Foo();
            var path = NavigationFactory.Create(root)
                .For(f => f.Bar)
                .For(b => b.Tet)
                .For(t => t.Kip);

            path.IsValid().Should().BeTrue();
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
