using Kzrnm.Competitive;
using Kzrnm.Competitive.Testing.Serializer;
using System;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(typeof(PointSerializer), [
    typeof(PointInt),
    typeof(PointLong),
    typeof(PointDouble),
    typeof(PointFraction),
])]


namespace Kzrnm.Competitive.Testing.Serializer;

public class PointSerializer : IXunitSerializer
{
    public object Deserialize(Type type, string serializedValue)
    {
        var g = serializedValue.Split('\0');
        if (type == typeof(PointInt))
            return new PointInt(Parse<int>(g[0]), Parse<int>(g[1]));
        if (type == typeof(PointLong))
            return new PointLong(Parse<long>(g[0]), Parse<long>(g[1]));
        if (type == typeof(PointDouble))
            return new PointDouble(Parse<double>(g[0]), Parse<double>(g[1]));
        if (type == typeof(PointFraction))
            return new PointFraction(Parse<Fraction>(g[0]), Parse<Fraction>(g[1]));
        throw new NotSupportedException();
    }

    public bool IsSerializable(Type type, object value, out string failureReason)
        => (failureReason = null) is null;

    public string Serialize(object value)
    {
        return value switch
        {
            PointInt pi => $"{pi.X}\0{pi.Y}",
            PointLong pl => $"{pl.X}\0{pl.Y}",
            PointDouble pd => $"{pd.X}\0{pd.Y}",
            PointFraction pf => $"{pf.X}\0{pf.Y}",
            _ => throw new NotSupportedException(),
        };
    }

    static T Parse<T>(string v) where T : IParsable<T>
        => T.Parse(v, null);
}
