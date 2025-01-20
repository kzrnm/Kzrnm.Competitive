using System;
using System.Linq;
using Xunit.Sdk;

namespace Kzrnm.Competitive.Testing.TwoDimensional
{
    public class PointFractionTests
    {
        public static TheoryData<PointFraction, PointFraction, Fraction, double> Distance_Data => new()
        {
            { new (0,0), new (0,0), 0, 0 },
            { new (1,1), new (1,1), 0, 0 },
            { new (0,0), new (1,1), 2, Math.Sqrt(2) },
            { new (0,0), new (-1,-1), 2, Math.Sqrt(2) },
            { new (-1,-2), new (3,5), 65, Math.Sqrt(65) },
        };
        [Theory]
        [MemberData(nameof(Distance_Data))]
        public void Distance(PointFraction p1, PointFraction p2, Fraction d2, double distance)
        {
            p1.Distance2(p2).ShouldBe(d2);
            p2.Distance2(p1).ShouldBe(d2);
            p1.Distance(p2).ShouldBe(distance);
            p2.Distance(p1).ShouldBe(distance);
        }

        public static PointFraction[] SortedPoints =>
        [
            new (0, 0),
            new (1, 0),
            new (5, 0),
            new (100000, 1),
            new (1, 1),
            new (1, 10000),
            new (0, 1),
            new (0, 55),
            new (-1, 10000),
            new (-1, 1),
            new (-1500, 1),
            new (-10000000, 1),
            new (-1, 0),
            new (-2, 0),
            new (-500, 0),
            new (-500, -1),
            new (-1, -1),
            new (-2, -2),
            new (-1, -50000),
            new (0, -1),
            new (0, -2000),
            new (1, -10),
            new (100000, -1),
        ];
        [Fact]
        public void CompareTo()
        {
            for (int i = 0; i < SortedPoints.Length; i++)
                for (int j = 0; j < SortedPoints.Length; j++)
                    SortedPoints[i].CompareTo(SortedPoints[j])
                        .ShouldBe(i.CompareTo(j), $"({SortedPoints[i]}).CompareTo(({SortedPoints[j]})) == {i}.CompareTo({j})");
        }

        public static TheoryData<PointFraction, PointFraction, Fraction> Inner_Data => new()
        {
            { new (0,0), new (0,0), 0 },
            { new (0,1), new (1,0), 0 },
            { new (5,1), new (-2,10), 0 },
            { new (10,0), new (-2,10), -20 },
            { new (5,3), new (-2,-7), -31 },
        };
        [Theory]
        [MemberData(nameof(Inner_Data))]
        public void Inner(PointFraction p1, PointFraction p2, Fraction expected)
        {
            p1.Inner(p2).ShouldBe(expected);
        }

        public static TheoryData<PointFraction, PointFraction, Fraction> Cross_Data => new()
        {
            { new (0,0), new (0,0), 0 },
            { new (0,1), new (1,0), -1 },
            { new (5,1), new (-2,10), 52 },
            { new (10,0), new (-2,10), 100 },
            { new (5,3), new (-2,-7), -29 },
        };
        [Theory]
        [MemberData(nameof(Cross_Data))]
        public void Cross(PointFraction p1, PointFraction p2, Fraction expected)
        {
            p1.Cross(p2).ShouldBe(expected);
        }

        public static TheoryData<PointFraction[], Fraction> Area_Data => new()
        {
            {
                new PointFraction[]
                {
                    new(1,1),
                    new(2,2),
                    new(1,3),
                    new(-1,1),
                },
                6
            },
            {
                new PointFraction[]
                {
                    new(-1,1),
                    new(1,3),
                    new(2,2),
                    new(1,1),
                },
                6
            },
            {
                new PointFraction[]
                {
                    new(new(11,10),new(51,100)),
                    new(new(195,100),new(64423,10000)),
                    new(new(4341,1000),new(12265,10000)),
                    new(new(9,2),4),
                },
                new(97425693,10000000)
            },
        };

        [Theory]
        [MemberData(nameof(Area_Data))]
        public void Area(PointFraction[] points, Fraction expected)
        {
            PointFraction.Area2(points).ShouldBe(expected);
            PointFraction.Area(points).ShouldBe(expected / 2);
        }

        [Fact]
        public void ConvexHull1()
        {
            var points = new PointFraction[]
            {
                (100000000, 100000000),
                ( 80000000,  90000000),
                ( 10000000, -10000000),
                (100000000,  80000000),
                ( 40000000,  20000000),
                ( 80000000,  60000000),
                ( 40000000,  80000000),
                ( 10000000,  50000000),
                (110000000, 100000000),
                ( 60000000,  80000000),
                ( 80000000,  80000000),
                (-10000000,  50000000),
            };
            PointFraction.ConvexHull(points)
                .Select(i => points[i])
                .ShouldBe([
                    (10000000, -10000000),
                    (40000000, 20000000),
                    (80000000, 60000000),
                    (100000000, 80000000),
                    (110000000, 100000000),
                    (100000000, 100000000),
                    (40000000, 80000000),
                    (-10000000, 50000000),
                ]);
        }

        [Fact]
        public void ConvexHull2()
        {
            var points = new PointFraction[]
            {
                (10, 10),
                ( 8,  9),
                ( 8,  8),
                (10,  8),
                ( 0,  0),
                ( 4,  0),
                ( 8,  6),
                ( 1,  5),
                ( 6,  8),
                (11, 10),
                ( 1,  8),
                (-6,  6),
                (-4,  4),
                (-1,  1)
            };
            PointFraction.ConvexHull(points)
                .Select(i => points[i])
                .ShouldBe([
                    (0, 0),
                    (4, 0),
                    (10, 8),
                    (11, 10),
                    (10, 10),
                    (1, 8),
                    (-6, 6),
                    (-4, 4),
                    (-1, 1),
                ]);
        }

        public static TheoryData<PointFraction, Fraction, Fraction, Fraction, double> 直線との距離_Data => new()
        {
            { new (0,0), 1, 1, -1, 0.7071067811865475 },
            { new (0,0), -1, -1, 2, 1.414213562373095 },
            { new (1,0), -1, -1, 2, 0.7071067811865475 },
            { new (-1,0), -1, -1, 2, 2.1213203435596424 },
            { new (1,1), -1, -1, 2, 0 },
        };
        [Theory]
        [MemberData(nameof(直線との距離_Data))]
        public void 直線との距離(PointFraction p, Fraction a, Fraction b, Fraction c, double expected)
        {
            p.直線との距離(a, b, c).ShouldBe(expected);
        }

        public static TheoryData<PointFraction, PointFraction, SerializableTuple<Fraction, Fraction, Fraction>> 直線_Data => new()
        {
            { new (0,0), new (1,1), (1, -1, 0) },
            { new (1,0), new (1,1), (1, 0, -1) },
            { new (0,1), new (1,1), (0, -1, 1) },
            { new (-1,10), new (10,2), (-8, -11, 102) },
        };
        [Theory]
        [MemberData(nameof(直線_Data))]
        public void 直線(PointFraction p1, PointFraction p2, SerializableTuple<Fraction, Fraction, Fraction> expected)
        {
            p1.直線(p2).ShouldBe(expected.ToTuple());
        }

        public static TheoryData<PointFraction, PointFraction, SerializableTuple<Fraction, Fraction, Fraction>> 垂直二等分線_Data => new()
        {
            { new (0,0), new (1,1), (-1, -1, 1) },
            { new (1,0), new (1,1), (0, -1, new(1,2)) },
            { new (0,1), new (1,1), (-1, 0, new(1,2)) },
            { new (-1,10), new (10,2), (-11, 8, new(3,2)) },
        };
        [Theory]
        [MemberData(nameof(垂直二等分線_Data))]
        public void 垂直二等分線(PointFraction p1, PointFraction p2, SerializableTuple<Fraction, Fraction, Fraction> expected)
        {
            p1.垂直二等分線(p2).ShouldBe(expected.ToTuple());
        }

        public static TheoryData<Fraction, Fraction, Fraction, Fraction, Fraction, Fraction, PointFraction> 直線と直線の交点_Data => new()
        {
            { 1, 1, 1, -1, 1, 2, new(new(1,2), new(-3,2)) },
            { -1, 5, new(1,2), -7, 7, new(58,10), new(new(51,56), new(23,280)) },
        };
        [Theory]
        [MemberData(nameof(直線と直線の交点_Data))]
        public void 直線と直線の交点(Fraction a, Fraction b, Fraction c, Fraction u, Fraction v, Fraction w, PointFraction expected)
        {
            PointFraction.直線と直線の交点(a, b, c, u, v, w).ShouldBe(expected);
        }

        public static TheoryData<Fraction, Fraction, PointFraction, SerializableTuple<Fraction, Fraction, Fraction>> 直線の垂線_Data => new()
        {
            { 1, 1, new (new(1,2), new(-3,2)), (1, -1, -2) },
            { 4, 7, new (-10, 2), (7, -4, 78) },
            { 0, 2, new (7, 5), (2, 0, -14) },
            { 2, 0, new (7, 5), (0, -2, 10) },
        };
        [Theory]
        [MemberData(nameof(直線の垂線_Data))]
        public void 直線の垂線(Fraction a, Fraction b, PointFraction p, SerializableTuple<Fraction, Fraction, Fraction> expected)
        {
            PointFraction.直線の垂線(a, b, p).ShouldBe(expected.ToTuple());
        }

        public static TheoryData<PointFraction, Fraction, PointFraction, Fraction, CirclePosition> 円の位置関係_Data => new()
        {
            { new (-1, -1), 10, new (1, 2), 1, CirclePosition.Inner },
            { new (5, 0), 5, new (1, 0), 1, CirclePosition.Inscribed },
            { new (-1, 0), new(12,10), new (1, 0), new(12,10), CirclePosition.Intersected },
            { new (0, 0), 1, new (1, 1), 1, CirclePosition.Intersected },
            { new (-1, 0), 1, new (1, 0), 1, CirclePosition.Circumscribed },
            { new (-1, 0), new(8,10), new (1, 0), 1, CirclePosition.Separated },
        };
        [Theory]
        [MemberData(nameof(円の位置関係_Data))]
        public void 円の位置関係(PointFraction p1, Fraction r1, PointFraction p2, Fraction r2, CirclePosition expected)
        {
            PointFraction.円の位置関係(p1, r1, p2, r2).ShouldBe(expected);
        }

        public static TheoryData<PointFraction, PointFraction, PointFraction, PointFraction, int> 線分が交差しているか_Data => new()
        {
            { new (-1, -1), new (1, 1), new (-1, 0), new (0, new(1,1000)), -1 },
            { new (-1, -1), new (1, 1), new (-1, 0), new (0, new(-1,1000)), 1 },
            { new (-1, -1), new (1, 1), new (-1, 0), new (0, 0), 0 },

            { new (-1, -1), new (1, 1), new (2, 2), new (3, 3), -1 },
            { new (-1, -1), new (1, 1), new (0, 0), new (3, 3), 1 },
            { new (-1, -1), new (1, 1), new (1, 1), new (3, 3), 0 },

            { new (-1, -1), new (-1, 1), new (-1, 2), new (-1, 3), -1 },
            { new (-1, -1), new (-1, 1), new (-1, 0), new (-1, 3), 1 },
            { new (-1, -1), new (-1, 1), new (-1, 1), new (-1, 3), 0 },

            { new (-1, 1), new (1, 1), new (2, 1), new (3, 1), -1 },
            { new (-1, 1), new (1, 1), new (0, 1), new (3, 1), 1 },
            { new (-1, 1), new (1, 1), new (1, 1), new (3, 1), 0 },
        };
        [Theory]
        [MemberData(nameof(線分が交差しているか_Data))]
        public void 線分が交差しているか(PointFraction a1, PointFraction b1, PointFraction a2, PointFraction b2, int expected)
        {
            PointFraction.線分が交差しているか(a1, b1, a2, b2).ShouldBe(expected);
            PointFraction.線分が交差しているか(b1, a1, a2, b2).ShouldBe(expected);
            PointFraction.線分が交差しているか(a1, b1, b2, a2).ShouldBe(expected);
            PointFraction.線分が交差しているか(b1, a1, b2, a2).ShouldBe(expected);

            PointFraction.線分が交差しているか(a2, b2, a1, b1).ShouldBe(expected);
            PointFraction.線分が交差しているか(b2, a2, a1, b1).ShouldBe(expected);
            PointFraction.線分が交差しているか(a2, b2, b1, a1).ShouldBe(expected);
            PointFraction.線分が交差しているか(b2, a2, b1, a1).ShouldBe(expected);
        }

        public class 三角形に分割Data : IXunitSerializable
        {
            public PointFraction[] Input { get; set; }
            public (PointFraction, PointFraction, PointFraction)[] Expected { get; set; }

            void IXunitSerializable.Deserialize(IXunitSerializationInfo info)
            {
                Input = info.GetValue<PointFraction[]>(nameof(Input));
                var ex = info.GetValue<PointFraction[]>(nameof(Expected));
                Expected = ex.Chunk(3).Select(t => (t[0], t[1], t[2])).ToArray();
            }

            void IXunitSerializable.Serialize(IXunitSerializationInfo info)
            {
                info.AddValue(nameof(Input), Input);
                info.AddValue(nameof(Expected), Expected.SelectMany(t => new[] { t.Item1, t.Item2, t.Item3 }).ToArray());
            }
        }
        public static TheoryData<三角形に分割Data> 三角形に分割_Data => new()
        {
            new 三角形に分割Data {
                Input =[],
                Expected =[],
            },
            new 三角形に分割Data {
                Input =new PointFraction[1],
                Expected =[],
            },
            new 三角形に分割Data {
                Input =new PointFraction[2],
                Expected =[],
            },
            new 三角形に分割Data {
                Input =
                [
                    new (0, 1),
                    new (0, 0),
                    new (1, 1),
                ],
                Expected =
                [
                    (new (0, 0), new (0, 1), new (1, 1)),
                ]
            },
            new 三角形に分割Data {
                Input =
                [
                    new (0, 0),
                    new (10, 0),
                    new (5, 5),
                    new (10, 10),
                    new (0, 10),
                ],
                Expected =
                [
                    (new (10, 0), new (0, 0), new (0, 10)),
                    (new (10, 0), new (0, 10), new (10, 10)),
                    (new (5, 5), new (10, 0), new (10, 10)),
                ]
            },
        };
        [Theory]
        [MemberData(nameof(三角形に分割_Data))]
        public void 三角形に分割(三角形に分割Data d)
        {
            PointFraction.三角形に分割(d.Input).ShouldBe(d.Expected);
        }

        [Fact]
        public void ConsoleWriter()
        {
            var utf8Wrapper = new Utf8ConsoleWriterWrapper();
            using (var cw = utf8Wrapper.GetWriter())
            {
                var arr = new PointFraction[]
                {
                    new(1,2),
                    new(new(1,2),new(4,3)),
                };
                cw.WriteLines(arr);
            }
            utf8Wrapper.Read().ShouldBe("""
            1/1 2/1
            1/2 4/3

            """.Replace("\r\n", "\n"));
        }
    }
}
