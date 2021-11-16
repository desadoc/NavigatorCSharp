using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Navigator.Tests
{
    public class NavigationGeneralTests
    {
        [Fact]
        public void ManyMethods_Default_WorksCorrectly()
        {
            var foo = new Foo
            {
                Bars = new List<Bar>
                {
                    new Bar
                    {
                        Tet1 = default,
                        Tet2 = new Tet
                        {
                            Kips = new List<Kip>
                            {
                                new Kip { Zuu = "Zuu1" },
                                new Kip { Zuu = "Zuu2" },
                                new Kip { Zuu = default },
                            }
                        },
                        Tet3 = new Tet
                        {
                            Kips = new List<Kip>
                            {
                                new Kip { Zuu = "Zuu3" },
                                default,
                                new Kip { Zuu = "Zuu4" },
                            }
                        }
                    },
                    new Bar
                    {
                        Tet1 = new Tet
                        {
                            Kips = new List<Kip>
                            {
                                new Kip { Zuu = "Zuu5" },
                                new Kip { Zuu = "Zuu6" },
                                new Kip { Zuu = default },
                            }
                        },
                        Tet2 = new Tet
                        {
                            Kips = new List<Kip>
                            {
                                new Kip { Zuu = "Zuu7" },
                                new Kip { Zuu = "Zuu8" },
                            }
                        },
                        Tet3 = new Tet
                        {
                            Kips = default
                        }
                    }
                }
            };

            var navigation = NavigationFactory.Create(foo)
                .ForEach(f => f.Bars)
                .SelectMany(bar => new[]
                {
                    bar.For(b => b.Tet1), bar.For(b => b.Tet2), bar.For(b => b.Tet3)
                })
                .SelectMany(tet => tet
                    .When(t => t.Kips != null && t.Kips.Count >= 3)
                    .ForEach(t => t.Kips))
                .Select(kip => kip.For(k => k.Zuu))
                .ToList();

            var zuus = navigation
                .Select(Zuu => Zuu.TryGetValue(out var value) ? value : "Invalid")
                .ToList();

            var paths = navigation
                .Select(Zuu => Zuu.GetPath())
                .ToList();

            zuus.Should().BeEquivalentTo(new[]
            {
                "Zuu1", "Zuu2", null, "Zuu3", "Invalid", "Zuu4", "Zuu5", "Zuu6", null
            }, options => options.WithStrictOrdering());

            paths.Should().BeEquivalentTo(new[]
            {
                "Bars[0].Tet2.Kips[0].Zuu", "Bars[0].Tet2.Kips[1].Zuu", "Bars[0].Tet2.Kips[2].Zuu",
                "Bars[0].Tet3.Kips[0].Zuu", "Bars[0].Tet3.Kips[1].Zuu", "Bars[0].Tet3.Kips[2].Zuu",
                "Bars[1].Tet1.Kips[0].Zuu", "Bars[1].Tet1.Kips[1].Zuu", "Bars[1].Tet1.Kips[2].Zuu",
            }, options => options.WithStrictOrdering());
        }

        private class Foo
        {
            public IReadOnlyList<Bar> Bars { get; set; }
        }

        private class Bar
        {
            public Tet Tet1 { get; set; }
            public Tet Tet2 { get; set; }
            public Tet Tet3 { get; set; }
        }

        private class Tet
        {
            public IReadOnlyList<Kip> Kips { get; set; }
        }

        private class Kip
        {
            public string Zuu { get; set; }
        }
    }
}
