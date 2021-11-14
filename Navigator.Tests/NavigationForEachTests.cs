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
                    new Bar { Tet = new Tet("First") },
                    new Bar { Tet = default },
                    new Bar { Tet = new Tet("Third") },
                }
            };

            var root = NavigationFactory.Create(foo);
            var tets = root.ForEach(f => f.Bars)
                .Select(bar => bar.For(b => b.Tet));

            tets.Should().OnlyContain(t => t.IsValid());

            var kips = tets
                .Select(tet => tet.GetValue());

            kips.Should().BeEquivalentTo(new[]
            {
                new Tet("First"), default, new Tet("Third")
            }, options => options.WithStrictOrdering());
        }

        [Fact]
        public void ForEach_PartiallyValidNavigation_CorrectValues()
        {
            var foo = new Foo
            {
                Bars = new List<Bar>
                {
                    new Bar { Tet = new Tet("First") },
                    new Bar { Tet = default },
                    new Bar { Tet = new Tet("Third") },
                }
            };

            var root = NavigationFactory.Create(foo);
            var kips = root.ForEach(f => f.Bars)
                .Select(bar => bar.For(b => b.Tet))
                .Select(tet => tet.For(t => t.Kip))
                .ToArray();

            kips[0].GetValue().Should().Be("First");
            kips[1].Invoking(t => t.GetValue()).Should().ThrowExactly<InvalidNavigationException>();
            kips[2].GetValue().Should().Be("Third");
        }

        [Fact]
        public void ForEach_InvalidNavigation_EmptyEnumerable()
        {
            var foo = new Foo();

            var root = NavigationFactory.Create(foo);
            var tets = root.ForEach(f => f.Bars)
                .Select(bar => bar.For(b => b.Tet))
                .Select(tet => tet.GetValue());

            tets.Should().BeEmpty();
        }

        [Fact]
        public void ForEach_EmptyCollection_EmptyEnumerable()
        {
            var foo = new Foo
            {
                Bars = new List<Bar>()
            };

            var root = NavigationFactory.Create(foo);
            var tets = root.ForEach(f => f.Bars)
                .Select(bar => bar.For(b => b.Tet))
                .Select(tet => tet.GetValue());

            tets.Should().BeEmpty();
        }

        [Fact]
        public void ForEach_NestedCollection_CorrectValues()
        {
            var goo = new Goo
            {
                Cars = new List<Car>
                {
                    new Car
                    {
                        Tets = new List<Tet>
                        {
                            new Tet("First"),
                            new Tet("Second"),
                        },
                    },
                    new Car
                    {
                        Tets = new List<Tet>
                        {
                            new Tet("Third"),
                        },
                    },
                    new Car
                    {
                        Tets = new List<Tet>
                        {
                            new Tet("Fourth"),
                            new Tet("Fifth"),
                        },
                    }
                }
            };

            var root = NavigationFactory.Create(goo);
            var kips = root.ForEach(g => g.Cars)
                .SelectMany(car => car.ForEach(c => c.Tets))
                .Select(tet => tet.For(t => t.Kip))
                .Select(t => t.GetValue());

            kips.Should().BeEquivalentTo(new[]
            {
                "First", "Second", "Third", "Fourth", "Fifth"
            }, options => options.WithStrictOrdering());
        }

        private class Foo
        {
            public IReadOnlyList<Bar> Bars { get; set; }
        }

        private class Bar
        {
            public Tet Tet { get; set; }
        }

        private class Tet
        {
            public string Kip { get; }

            public Tet(string kip)
            {
                Kip = kip;
            }
        }

        private class Goo
        {
            public IReadOnlyList<Car> Cars { get; set; }
        }

        private class Car
        {
            public IReadOnlyList<Tet> Tets { get; set; }
        }
    }
}
