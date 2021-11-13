using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Navigator.Tests
{
    public class NavigationForEachTests
    {
        [Fact]
        public void ForEach_ValidNavigation_CorrectValues()
        {
            var foo = new Foo
            {
                Bars = new List<Bar>
                {
                    new Bar { Tet = "First" },
                    new Bar { Tet = "Second" },
                    new Bar { Tet = "Third" },
                }
            };

            var root = NavigationFactory.Create(foo);
            var tets = root.ForEach(f => f.Bars)
                .Select(bar => bar.For(b => b.Tet))
                .Select(tet => tet.GetValue());

            tets.Should().BeEquivalentTo(new[]
            {
                "First", "Second", "Third"
            }, options => options.WithStrictOrdering());
        }

        private class Foo
        {
            public IReadOnlyList<Bar> Bars { get; set; }
        }

        private class Bar
        {
            public string Tet { get; set; } = string.Empty;
        }
    }
}
