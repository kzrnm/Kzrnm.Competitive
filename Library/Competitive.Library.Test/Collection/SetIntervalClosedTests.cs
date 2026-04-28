namespace Kzrnm.Competitive.Testing.Collection;

public class SetIntervalClosedTests
{
    public static IEnumerable<((int, int)[], (int, int)[])> Add_Data =>
    [
        ([], []),
        (
            [
                (101,101),
                (15, 100),
                (-20, -2),
                (0, 10),
            ],
            [
                (-20, -2),
                (0, 10),
                (15, 100),
                (101,101),
            ]
        ),
        (
            [
                (9, 100),
                (-20, 2),
                (0, 10),
            ],
            [
                (-20, 100),
            ]
        ),
        (
            [
                (50, 60),
                (10, 20),
                (30, 40),
            ],
            [
                (10, 20),
                (30, 40),
                (50, 60),
            ]
        ),
        (
            [
                (50, 60),
                (10, 20),
                (30, 40),
                (15, 25),
            ],
            [
                (10, 25),
                (30, 40),
                (50, 60),
            ]
        ),
        (
            [
                (50, 60),
                (10, 20),
                (30, 40),
                (15, 25),
                (25, 35),
            ],
            [
                (10, 40),
                (50, 60),
            ]
        ),
        (
            [
                (50, 60),
                (10, 20),
                (30, 40),
                (15, 25),
                (25, 35),
                (10, 41),
            ],
            [
                (10, 41),
                (50, 60),
            ]
        ),
        (
            [
                (50, 60),
                (10, 20),
                (30, 40),
                (15, 25),
                (25, 35),
                (10, 41),
                (49, 60),
            ],
            [
                (10, 41),
                (49, 60),
            ]
        ),
        (
            [
                (50, 60),
                (10, 20),
                (30, 40),
                (15, 25),
                (25, 35),
                (10, 41),
                (49, 60),
                (9, 61),
            ],
            [
                (9, 61),
            ]
        ),
        (
            [
                (10, 1000),
                (20, 0),
                (100, 900),
                (100, 900),
            ],
            [
                (10, 1000),
            ]
        ),
        (
            [
                (10, 100),
                (-20, 0),
                (0, 10),
            ],
            [
                (-20, 100),
            ]
        ),
        (
            [
                (-20, 190),
                (-1000, 1000),
                (-30, 100),
            ],
            [
                (-1000, 1000),
            ]
        ),
        (
            [
                (-30, 100),
                (-10, 1000),
                (-20, 190),
            ],
            [
                (-30, 1000),
            ]
        ),
        (
            [
                (-10, 0),
                (10, 20),
                (30, 40),
                (0, 30),
            ],
            [
                (-10, 40),
            ]
        ),
        (
            [
                (-10, 0),
                (10, 20),
                (30, 40),
                (-1, 31),
            ],
            [
                (-10, 40),
            ]
        ),
        (
            [
                (-10, 0),
                (10, 20),
                (30, 40),
                (1, 29),
            ],
            [
                (-10, 0),
                (1, 29),
                (30, 40),
            ]
        ),
        (
            [
                (0, 0),
                (0, 0),
                (0, 0),
            ],
            [
                (0, 0),
            ]
        ),
        (
            [
                (99, 100),
                (98, 100),
                (97, 100),
                (96, 100),
                (95, 100),
                (101, 102),
                (101, 103),
                (101, 104),
                (101, 105),
                (101, 106),
            ],
            [
                (95, 100),
                (101, 106),
            ]
        ),
        (
            [
                (0, 9),
                (10, 20),
                (30, 40),
                (10, 25),
            ],
            [
                (0, 9),
                (10, 25),
                (30, 40),
            ]
        ),
        (
            [
                (0, 9),
                (20, 29),
                (30, 40),
                (10, 29),
            ],
            [
                (0, 9),
                (10, 29),
                (30, 40),
            ]
        )
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Add_Data))]
    public async Task Contructor((int, int)[] arg, (int, int)[] result)
    {
        await new SetIntervalClosedInt(arg).Should().BeEquivalentOrderTo(result);
    }

    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Add_Data))]
    public async Task AddTheory((int, int)[] arg, (int, int)[] result)
    {
        var set = new SetIntervalClosedInt();
        foreach (var (f, t) in arg)
            set.Add(f, t);
        await set.Should().BeEquivalentOrderTo(result);
    }
    [Test, MultipleAssertions]
    public async Task Add()
    {
        var set = new SetIntervalClosedInt();
        await set.Should().BeEquivalentOrderTo(new (int, int)[0]);

        set.Add(50, 60);
        await set.Should().BeEquivalentOrderTo([
            (50, 60),
        ]);

        set.Add(10, 20);
        await set.Should().BeEquivalentOrderTo([
            (10, 20),
            (50, 60),
        ]);

        set.Add(30, 40);
        await set.Should().BeEquivalentOrderTo([
            (10, 20),
            (30, 40),
            (50, 60),
        ]);

        set.Add(15, 25);
        await set.Should().BeEquivalentOrderTo([
            (10, 25),
            (30, 40),
            (50, 60),
        ]);

        set.Add(25, 30);
        await set.Should().BeEquivalentOrderTo([
            (10, 40),
            (50, 60),
        ]);

        set.Add(10, 41);
        await set.Should().BeEquivalentOrderTo([
            (10, 41),
            (50, 60),
        ]);

        set.Add(49, 60);
        await set.Should().BeEquivalentOrderTo([
            (10, 41),
            (49, 60),
        ]);

        set.Add(42, 48);
        await set.Should().BeEquivalentOrderTo([
            (10, 41),
            (42, 48),
            (49, 60),
        ]);

        set.Add(9, 61);
        await set.Should().BeEquivalentOrderTo([
            (9, 61),
        ]);

        set.Add(70, 80);
        await set.Should().BeEquivalentOrderTo([
            (9, 61),
            (70,80),
        ]);

        set.Add(5, 70);
        await set.Should().BeEquivalentOrderTo([
            (5,80),
        ]);
    }

    [Test, MultipleAssertions]
    public async Task MinMax()
    {
        var set = new SetIntervalClosedInt();
        await set.Should().BeEquivalentOrderTo(new (int, int)[0]);
        await set.Min.Should().BeEqualTo(default);
        await set.Max.Should().BeEqualTo(default);
        set.Add(50, 60);
        await set.Min.Should().BeEqualTo((50, 60));
        await set.Max.Should().BeEqualTo((50, 60));
        set.Add(10, 20);
        await set.Min.Should().BeEqualTo((10, 20));
        await set.Max.Should().BeEqualTo((50, 60));
        set.Add(30, 40);
        await set.Min.Should().BeEqualTo((10, 20));
        await set.Max.Should().BeEqualTo((50, 60));
        set.Add(15, 25);
        await set.Min.Should().BeEqualTo((10, 25));
        await set.Max.Should().BeEqualTo((50, 60));
        set.Add(25, 35);
        await set.Min.Should().BeEqualTo((10, 40));
        await set.Max.Should().BeEqualTo((50, 60));
        set.Add(10, 41);
        await set.Min.Should().BeEqualTo((10, 41));
        await set.Max.Should().BeEqualTo((50, 60));
        set.Add(49, 60);
        await set.Min.Should().BeEqualTo((10, 41));
        await set.Max.Should().BeEqualTo((49, 60));
        set.Add(9, 61);
        await set.Min.Should().BeEqualTo((9, 61));
        await set.Max.Should().BeEqualTo((9, 61));
    }


    public static IEnumerable<(int, int, bool, (int, int)[])> Remove_Data =>
    [
        (1,9,false, [(10, 20),(25, 30),(35, 40),(50, 60)]),
        (21,24,false, [(10, 20),(25, 30),(35, 40),(50, 60)]),
        (1,10,true, [(11, 20),(25, 30),(35, 40),(50, 60)]),
        (1,20,true, [(25, 30),(35, 40),(50, 60)]),
        (20,25,true, [(10, 19),(26, 30),(35, 40),(50, 60)]),
        (20,39,true, [(10, 19),(40, 40),(50, 60)]),
        (10,60,true,[]),
        (1,61,true,[]),
    ];
    [Test, MultipleAssertions]
    [MethodDataSource(nameof(Remove_Data))]
    public async Task Remove(int from, int to, bool success, (int, int)[] result)
    {
        var set = new SetIntervalClosedInt([
            (10, 20),
            (25, 30),
            (35, 40),
            (50, 60)]);
        await set.Remove(from, to).Should().BeEqualTo(success);
        await set.Should().BeEquivalentOrderTo(result);
    }

    [Test, MultipleAssertions]
    [Arguments(1, false)]
    [Arguments(8, false)]
    [Arguments(9, false)]
    [Arguments(10, true)]
    [Arguments(18, true)]
    [Arguments(19, true)]
    [Arguments(20, true)]
    [Arguments(28, false)]
    [Arguments(29, false)]
    [Arguments(30, true)]
    [Arguments(38, true)]
    [Arguments(39, true)]
    [Arguments(40, true)]
    [Arguments(48, false)]
    [Arguments(49, false)]
    [Arguments(50, true)]
    [Arguments(58, true)]
    [Arguments(59, true)]
    [Arguments(60, true)]
    [Arguments(68, false)]
    [Arguments(69, false)]
    public async Task Contains(int value, bool isContains)
    {
        var set = new SetIntervalClosedInt([
            (10, 20),
            (30, 40),
            (50, 60)]);
        await set.Contains(value).Should().BeEqualTo(isContains);
        if (isContains)
            await set.FindNode(value).Should().NotBeNull();
        else
            await set.FindNode(value).Should().BeNull();
    }


    [Test]
    [Arguments(1, 10, false)]
    [Arguments(1, 11, false)]
    [Arguments(1, 20, false)]
    [Arguments(9, 20, false)]
    [Arguments(10, 20, true)]
    [Arguments(10, 19, true)]
    [Arguments(11, 19, true)]
    public async Task ContainsRange(int from, int to, bool isContains)
    {
        var set = new SetIntervalClosedInt([
            (10, 20),
            (30, 40),
            (50, 60)]);
        await ((ICollection<(int, int)>)set).Contains((from, to)).Should().BeEqualTo(isContains);
    }

    public static IEnumerable<(int, int, (int, int)[])> RangeTruncate_Data =>
    [
        ( 0, 9, [] ),
        (21, 29, [] ),
        (61, 70, [] ),
        ( 0, 10, [(10, 10)]),
        (60, 70, [(60, 60)]),
        (10, 20, [(10, 20)]),
        (10, 30, [(10, 20), (30, 30)]),
        (10, 35, [(10, 20), (30, 35)]),
        (15, 60, [(15, 20), (30, 40), (50, 60)]),
    ];
    [Test]
    [MethodDataSource(nameof(RangeTruncate_Data))]
    public async Task RangeTruncate(int from, int to, (int, int)[] expected)
    {
        var set = new SetIntervalClosedInt([
            (10, 20),
            (30, 40),
            (50, 60)]);
        await set.RangeTruncate(from, to).Should().BeEquivalentOrderTo(expected);
    }

    public static IEnumerable<(int, int, (int, int)[])> RangeAll_Data =>
    [
        ( 0, 9, [] ),
        (21, 29, [] ),
        (61, 70, [] ),
        ( 0, 10, [(10, 20)]),
        (60, 70, [(50, 60)]),
        (10, 20, [(10, 20)]),
        (10, 30, [(10, 20), (30, 40)]),
        (10, 35, [(10, 20), (30, 40)]),
        (15, 60, [(10, 20), (30, 40), (50, 60)]),
    ];
    [Test]
    [MethodDataSource(nameof(RangeAll_Data))]
    public async Task RangeAll(int from, int to, (int, int)[] expected)
    {
        var set = new SetIntervalClosedInt([(10, 20), (30, 40), (50, 60)]);
        await set.RangeAll(from, to).Should().BeEquivalentOrderTo(expected);
    }

    [Test]
    public async Task UnionWith()
    {
        var set = new SetIntervalClosedInt([
            (10, 20),
            (30, 40),
            (50, 60),
            (100, 115)]);
        set.UnionWith([
            (7, 12),
            (22, 25),
            (40, 75),
        ]);
        await set.Should().BeEquivalentOrderTo([
            (7, 20),
            (22, 25),
            (30, 75),
            (100, 115),
        ]);
    }

    [Test]
    public async Task ExceptWith()
    {
        var set = new SetIntervalClosedInt([
            (-10,-4),
            (10, 20),
            (30, 40),
            (50, 60),
            (100, 115)]);
        set.ExceptWith([
            (-10,-4),
            (7, 12),
            (22, 25),
            (26, 44),
            (49, 105),
        ]);
        await set.Should().BeEquivalentOrderTo([
            (13, 20),
            (106, 115),
        ]);
    }

    [Test]
    public async Task IntersectWith()
    {
        var set = new SetIntervalClosedInt([
            (-10,-4),
            (10, 20),
            (30, 40),
            (50, 60),
            (100, 115)]);
        set.IntersectWith([
            (-10,-4),
            (7, 12),
            (22, 25),
            (26, 44),
            (49, 105),
        ]);
        await set.Should().BeEquivalentOrderTo([
            (-10, -4),
            (10, 12),
            (30, 40),
            (50, 60),
            (100, 105),
        ]);
    }
}