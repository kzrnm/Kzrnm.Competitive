using Kzrnm.Competitive;
using Kzrnm.Competitive.Testing;
using Kzrnm.Competitive.Testing.Serializer;
using System;
using System.Reflection;
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

public partial class ParserSerializer : IXunitSerializer
{
    public object Deserialize(Type type, string serializedValue)
    {
        return Parse(type, serializedValue);
    }

    public bool IsSerializable(Type type, object value, out string failureReason)
    {
        failureReason = null;
        if (ParseMethod(type) == null)
        {
            failureReason = "Not IParsable<T>";
            return false;
        }
        return true;
    }

    public string Serialize(object value)
    {
        return value.ToString();
    }

    static MethodInfo ParseMethod(Type type)
        => type.GetMethod(
            nameof(IParsable<int>.Parse),
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static,
            [typeof(string), typeof(IFormatProvider)]);
    static object Parse(Type type, string v)
        => ParseMethod(type).Invoke(null, [v, null]);
}
