using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit.Sdk;

namespace Kzrnm.Competitive.Testing;
public record struct SerializableTuple<T1, T2>(T1 Item1, T2 Item2) : ITuple, IXunitSerializable
{
    readonly object ITuple.this[int index] => index switch
    {
        0 => Item1,
        1 => Item2,
        _ => throw new IndexOutOfRangeException(),
    };
    readonly int ITuple.Length => 2;
    public static implicit operator SerializableTuple<T1, T2>(ValueTuple<T1, T2> t) => new(t.Item1, t.Item2);
    public static implicit operator ValueTuple<T1, T2>(SerializableTuple<T1, T2> t) => t.ToTuple();
    public readonly ValueTuple<T1, T2> ToTuple() => new(Item1, Item2);
    public readonly void Deconstruct(out T1 item1, out T2 item2)
    {
        item1 = Item1;
        item2 = Item2;
    }
    public readonly override string ToString() => $"({Item1}, {Item2})";

    void IXunitSerializable.Deserialize(IXunitSerializationInfo info)
    {
        Item1 = info.GetValue<T1>(nameof(T1));
        Item2 = info.GetValue<T2>(nameof(T2));
    }
    readonly void IXunitSerializable.Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(T1), Item1);
        info.AddValue(nameof(T2), Item2);
    }
}

public record struct SerializableTuple<T1, T2, T3>(T1 Item1, T2 Item2, T3 Item3) : ITuple, IXunitSerializable
{
    readonly object ITuple.this[int index] => index switch
    {
        0 => Item1,
        1 => Item2,
        2 => Item3,
        _ => throw new IndexOutOfRangeException(),
    };
    readonly int ITuple.Length => 3;
    public static implicit operator SerializableTuple<T1, T2, T3>(ValueTuple<T1, T2, T3> t) => new(t.Item1, t.Item2, t.Item3);
    public static implicit operator ValueTuple<T1, T2, T3>(SerializableTuple<T1, T2, T3> t) => t.ToTuple();
    public readonly ValueTuple<T1, T2, T3> ToTuple() => new(Item1, Item2, Item3);
    public readonly void Deconstruct(out T1 item1, out T2 item2, out T3 item3)
    {
        item1 = Item1;
        item2 = Item2;
        item3 = Item3;
    }
    public readonly override string ToString() => $"({Item1}, {Item2}, {Item3})";

    void IXunitSerializable.Deserialize(IXunitSerializationInfo info)
    {
        Item1 = info.GetValue<T1>(nameof(T1));
        Item2 = info.GetValue<T2>(nameof(T2));
        Item3 = info.GetValue<T3>(nameof(T3));
    }
    readonly void IXunitSerializable.Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(T1), Item1);
        info.AddValue(nameof(T2), Item2);
        info.AddValue(nameof(T3), Item3);
    }
}
public record struct SerializableTuple<T1, T2, T3, T4>(T1 Item1, T2 Item2, T3 Item3, T4 Item4) : ITuple, IXunitSerializable
{
    readonly object ITuple.this[int index] => index switch
    {
        0 => Item1,
        1 => Item2,
        2 => Item3,
        3 => Item4,
        _ => throw new IndexOutOfRangeException(),
    };
    readonly int ITuple.Length => 4;
    public static implicit operator SerializableTuple<T1, T2, T3, T4>(ValueTuple<T1, T2, T3, T4> t) => new(t.Item1, t.Item2, t.Item3, t.Item4);
    public static implicit operator ValueTuple<T1, T2, T3, T4>(SerializableTuple<T1, T2, T3, T4> t) => t.ToTuple();
    public readonly ValueTuple<T1, T2, T3, T4> ToTuple() => new(Item1, Item2, Item3, Item4);
    public readonly void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4)
    {
        item1 = Item1;
        item2 = Item2;
        item3 = Item3;
        item4 = Item4;
    }
    public readonly override string ToString() => $"({Item1}, {Item2}, {Item3}, {Item4})";

    void IXunitSerializable.Deserialize(IXunitSerializationInfo info)
    {
        Item1 = info.GetValue<T1>(nameof(T1));
        Item2 = info.GetValue<T2>(nameof(T2));
        Item3 = info.GetValue<T3>(nameof(T3));
        Item4 = info.GetValue<T4>(nameof(T4));
    }
    readonly void IXunitSerializable.Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(T1), Item1);
        info.AddValue(nameof(T2), Item2);
        info.AddValue(nameof(T3), Item3);
        info.AddValue(nameof(T4), Item4);
    }
}
public static class SerializableTupleExtensions
{
    public static ValueTuple<T1, T2>[] ToTuple<T1, T2>(this IEnumerable<SerializableTuple<T1, T2>> tt)
        => tt.Select(t => t.ToTuple()).ToArray();
    public static ValueTuple<T1, T2, T3>[] ToTuple<T1, T2, T3>(this IEnumerable<SerializableTuple<T1, T2, T3>> tt)
        => tt.Select(t => t.ToTuple()).ToArray();
    public static ValueTuple<T1, T2, T3, T4>[] ToTuple<T1, T2, T3, T4>(this IEnumerable<SerializableTuple<T1, T2, T3, T4>> tt)
        => tt.Select(t => t.ToTuple()).ToArray();
}
