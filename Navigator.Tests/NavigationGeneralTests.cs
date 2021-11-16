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
                                new Kip { Zuu = "Buu1" },
                                new Kip { Zuu = "Buu2" },
                                new Kip { Zuu = default },
                            }
                        },
                        Tet3 = new Tet
                        {
                            Kips = new List<Kip>
                            {
                                new Kip { Zuu = "Buu3" },
                                default,
                                new Kip { Zuu = "Buu4" },
                            }
                        }
                    },
                    new Bar
                    {
                        Tet1 = new Tet
                        {
                            Kips = new List<Kip>
                            {
                                new Kip { Zuu = "Buu5" },
                                new Kip { Zuu = "Buu6" },
                                new Kip { Zuu = default },
                            }
                        },
                        Tet2 = new Tet
                        {
                            Kips = new List<Kip>
                            {
                                new Kip { Zuu = "Buu7" },
                                new Kip { Zuu = "Buu8" },
                            }
                        },
                        Tet3 = new Tet
                        {
                            Kips = new List<Kip>
                            {
                                new Kip { Zuu = "Buu9" },
                            }
                        }
                    }
                }
            };

            var zuus = NavigationFactory.Create(foo)
                .ForEach(f => f.Bars)
                .SelectMany(bar => new[]
                {
                    bar.For(b => b.Tet1), bar.For(b => b.Tet2), bar.For(b => b.Tet3)
                })
                .SelectMany(tet => tet
                    .When(t => t.Kips != null && t.Kips.Count >= 3)
                    .ForEach(t => t.Kips))
                .Select(kip => kip.For(k => k.Zuu))
                .Select(buu => buu.TryGetValue(out var value) ? value : "Invalid");

            zuus.Should().BeEquivalentTo(new[]
            {
                "Buu1", "Buu2", null, "Buu3", "Invalid", "Buu4", "Buu5", "Buu6", null
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
