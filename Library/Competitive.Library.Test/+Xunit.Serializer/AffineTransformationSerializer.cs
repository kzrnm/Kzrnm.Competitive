using Kzrnm.Competitive.Testing.Serializer;
using System;
using System.Reflection;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(typeof(AffineTransformationSerializer), [
    typeof(DoubleAffineTransformation),
    typeof(Mod998244353AffineTransformation),
])]


namespace Kzrnm.Competitive.Testing.Serializer;

public partial class AffineTransformationSerializer : IXunitSerializer
{
    public object Deserialize(Type type, string serializedValue)
    {
        var s = serializedValue.Split('\0');
        var t = type.GenericTypeArguments[0];
        var a = Parse(t, s[0]);
        var b = Parse(t, s[1]);
        return type.GetConstructor([t, t]).Invoke([a, b]);
    }

    public bool IsSerializable(Type type, object value, out string failureReason)
        => (failureReason = null) == null;

    public string Serialize(object value)
    {
        var a = value.GetType().GetProperty(nameof(AffineTransformation<int>.a)).GetValue(value);
        var b = value.GetType().GetProperty(nameof(AffineTransformation<int>.b)).GetValue(value);
        return $"{a}\0{b}";
    }

    static MethodInfo ParseMethod(Type type)
        => type.GetMethod(
            nameof(IParsable<int>.Parse),
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static,
            [typeof(string), typeof(IFormatProvider)]);
    static object Parse(Type type, string v)
        => ParseMethod(type).Invoke(null, [v, null]);
}
