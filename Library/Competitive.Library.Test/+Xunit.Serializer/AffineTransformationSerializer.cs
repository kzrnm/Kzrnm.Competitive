using AtCoder;
using Kzrnm.Competitive.Testing.Serializer;
using System;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(typeof(AffineTransformationSerializer<double>), typeof(DoubleAffineTransformation))]
[assembly: RegisterXunitSerializer(typeof(AffineTransformationSerializer<StaticModInt<Mod998244353>>), typeof(Mod998244353AffineTransformation))]


namespace Kzrnm.Competitive.Testing.Serializer;

public class AffineTransformationSerializer<T> : IXunitSerializer where T : IParsable<T>
{
    public object Deserialize(Type type, string serializedValue)
    {
        var s = serializedValue.Split('\0');
        var t = type.GenericTypeArguments[0];
        var a = T.Parse(s[0], null);
        var b = T.Parse(s[1], null);
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
}
