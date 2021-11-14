using FluentAssertions;
using Xunit;

namespace Navigator.Tests
{
    public class NavigationGetValueTests
    {
        [Fact]
        public void GetValue_ValidNavigation_CorrectValue()
        {
            var root = new Foo();
            var path = NavigationFactory.Create(root)
                .For(f => f.Bar.Tet.Kip);

            path.GetValue().Should().Be("Kip!");
            path.GetPath().Should().Be("Bar.Tet.Kip");
        }

        [Fact]
        public void GetValue_ValidCompositePath_CorrectValue()
        {
            var root = new Foo();
            var path = NavigationFactory.Create(root)
                .For(f => f.Bar)
                .For(b => b.Tet)
                .For(t => t.Kip);

            path.GetValue().Should().Be("Kip!");
            path.GetPath().Should().Be("Bar.Tet.Kip");
        }

        [Fact]
        public void GetValue_InvalidNavigation_ThrowsException()
        {
            var root = new Foo { Bar = default };
            var path = NavigationFactory.Create(root)
                .For(f => f.Bar.Tet.Kip);

            path.Invoking(p => p.GetValue()).Should().ThrowExactly<InvalidNavigationException>();
            path.GetPath().Should().Be("Bar.Tet.Kip");
        }

        [Fact]
        public void GetValue_InvalidCompositeNavigation_ThrowsException()
        {
            var root = new Foo { Bar = default };
            var path = NavigationFactory.Create(root)
                .For(f => f.Bar)
                .For(b => b.Tet)
                .For(t => t.Kip);

            path.Invoking(p => p.GetValue()).Should().ThrowExactly<InvalidNavigationException>();
            path.GetPath().Should().Be("Bar.Tet.Kip");
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
