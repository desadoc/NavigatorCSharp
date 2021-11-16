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
                                new Kip { Buu = "Buu1" },
                                new Kip { Buu = "Buu2" },
                                new Kip { Buu = default },
                            }
                        },
                        Tet3 = new Tet
                        {
                            Kips = new List<Kip>
                            {
                                new Kip { Buu = "Buu3" },
                                default,
                                new Kip { Buu = "Buu4" },
                            }
                        }
                    },
                    new Bar
                    {
                        Tet1 = new Tet
                        {
                            Kips = new List<Kip>
                            {
                                new Kip { Buu = "Buu5" },
                                new Kip { Buu = "Buu6" },
                                new Kip { Buu = default },
                            }
                        },
                        Tet2 = new Tet
                        {
                            Kips = new List<Kip>
                            {
                                new Kip { Buu = "Buu7" },
                                new Kip { Buu = "Buu8" },
                            }
                        },
                        Tet3 = new Tet
                        {
                            Kips = new List<Kip>
                            {
                                new Kip { Buu = "Buu9" },
                            }
                        }
                    }
                }
            };

            var buus = NavigationFactory.Create(foo)
                .ForEach(f => f.Bars)
                .SelectMany(bar => new[]
                {
                    bar.For(b => b.Tet1), bar.For(b => b.Tet2), bar.For(b => b.Tet3)
                })
                .SelectMany(tet => tet
                    .When(t => t.Kips != null && t.Kips.Count >= 3)
                    .ForEach(t => t.Kips))
                .Select(kip => kip.For(k => k.Buu))
                .Select(buu => buu.TryGetValue(out var value) ? value : "Invalid");

            buus.Should().BeEquivalentTo(new[]
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
            public string Buu { get; set; }
        }
    }
}
