using Kzrnm.Competitive;
using Kzrnm.Competitive.Testing.Serializer;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(typeof(BitMatrixSerializer), [
    typeof(BitMatrix),
    typeof(BitOrMatrix),
])]
[assembly: RegisterXunitSerializer(typeof(BitMatrixTSerializer), [typeof(BitMatrix64), typeof(BitMatrix<UInt128>)])]


namespace Kzrnm.Competitive.Testing.Serializer;

public class BitMatrixSerializer : IXunitSerializer
{
    readonly BitArraySerializer BitArraySerializer = new();
    public object Deserialize(Type type, string serializedValue)
    {
        if (type != typeof(BitMatrix) && type != typeof(BitOrMatrix))
            throw new NotSupportedException();
        switch (serializedValue)
        {
            case nameof(Internal.ArrayMatrixKind.Identity):
                return type == typeof(BitOrMatrix)
                ? BitOrMatrix.Identity
                : BitMatrix.Identity;
            case nameof(Internal.ArrayMatrixKind.Zero):
                return type == typeof(BitOrMatrix)
                ? BitOrMatrix.Zero
                : BitMatrix.Zero;
        }

        var sp = serializedValue.Split('\0');
        var b = new BitArray[sp.Length];
        for (int i = 0; i < b.Length; i++)
        {
            b[i] = (BitArray)BitArraySerializer.Deserialize(typeof(BitArray), sp[i]);
        }
        return type == typeof(BitOrMatrix)
                ? new BitOrMatrix(b)
                : new BitMatrix(b);
    }

    public bool IsSerializable(Type type, object value, out string failureReason)
        => (failureReason = null) is null;

    public string Serialize(object value)
    {
        var (kind, v) = value switch
        {
            BitMatrix b => (b.kind, b._v),
            BitOrMatrix b => (b.kind, b._v),
            _ => throw new NotSupportedException(),
        };
        if (kind != Internal.ArrayMatrixKind.Normal)
            return kind.ToString();
        return string.Join('\0', v.Select(BitArraySerializer.Serialize));
    }
}

public class BitMatrixTSerializer : IXunitSerializer
{
    public object Deserialize(Type type, string serializedValue)
    {
        if (type != typeof(BitMatrix<>).MakeGenericType(type.GenericTypeArguments))
            throw new NotSupportedException();
        switch (serializedValue)
        {
            case nameof(Internal.ArrayMatrixKind.Identity):
            case nameof(Internal.ArrayMatrixKind.Zero):
                return type.GetField(serializedValue, BindingFlags.Static | BindingFlags.Public)
                    .GetValue(null);
        }

        var sp = serializedValue.Split('\0');
        var b = Array.CreateInstance(type.GenericTypeArguments[0], sp.Length);
        for (int i = 0; i < b.Length; i++)
        {
            if (type == typeof(BitMatrix64)) b.SetValue(ulong.Parse(sp[i]), i);
            else if (type == typeof(BitMatrix<UInt128>)) b.SetValue(ulong.Parse(sp[i]), i);
            else throw new FormatException();
        }
        return type.GetConstructor([b.GetType()]).Invoke([b]);
    }

    public bool IsSerializable(Type type, object value, out string failureReason)
        => (failureReason = null) is null;

    public string Serialize(object value)
    {
        var kind = (Internal.ArrayMatrixKind)value.GetType().GetField(nameof(BitMatrix<uint>.kind), BindingFlags.Instance | BindingFlags.NonPublic)
            .GetValue(value);
        if (kind != Internal.ArrayMatrixKind.Normal)
            return kind.ToString();
        var v = (IEnumerable)value.GetType().GetField(nameof(BitMatrix<uint>._v), BindingFlags.Instance | BindingFlags.Public)
            .GetValue(value);
        return string.Join('\0', v.Cast<object>().Select(v => v.ToString()));
    }
}
