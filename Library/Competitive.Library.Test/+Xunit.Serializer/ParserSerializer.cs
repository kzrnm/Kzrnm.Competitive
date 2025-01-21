using Kzrnm.Competitive;
using Kzrnm.Competitive.Testing;
using Kzrnm.Competitive.Testing.Serializer;
using System;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(typeof(ParserSerializer), [
    typeof(UInt24),
    typeof(Int128),
    typeof(UInt128),
    typeof(UInt256),
    typeof(UInt512),
    typeof(Fraction),
])]


namespace Kzrnm.Competitive.Testing.Serializer;

public class ParserSerializer : IXunitSerializer
{
    public object Deserialize(Type type, string serializedValue)
    {
        if (type == typeof(UInt24) && UInt24.TryParse(serializedValue, out var vUInt24)) return vUInt24;
        else if (type == typeof(Int128) && Int128.TryParse(serializedValue, out var vInt128)) return vInt128;
        else if (type == typeof(UInt128) && UInt128.TryParse(serializedValue, out var vUInt128)) return vUInt128;
        else if (type == typeof(UInt256) && UInt256.TryParse(serializedValue, out var vUInt256)) return vUInt256;
        else if (type == typeof(UInt512) && UInt512.TryParse(serializedValue, out var vUInt512)) return vUInt512;
        else if (type == typeof(Fraction) && Fraction.TryParse(serializedValue, out var vFraction)) return vFraction;
        throw new FormatException();
    }

    public bool IsSerializable(Type type, object value, out string failureReason)
        => (failureReason = null) == null;

    public string Serialize(object value)
    {
        return value.ToString();
    }
}
