using AtCoder;
using Kzrnm.Competitive;
using Kzrnm.Competitive.Testing.Serializer;
using System;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(typeof(ParsableSerializer), [
    typeof(MontgomeryModInt<Mod998244353>),
])]

namespace Kzrnm.Competitive.Testing.Serializer;

public class ParsableSerializer : IXunitSerializer
{
    public object Deserialize(Type type, string serializedValue)
    {
        if (!type.IsAssignableTo(typeof(IParsable<>).MakeGenericType(type)))
            throw new NotSupportedException();

        return GetType().GetMethod(nameof(Parse))
            .MakeGenericMethod(type)
            .Invoke(null, [serializedValue]);
    }

    public bool IsSerializable(Type type, object value, out string failureReason)
        => (failureReason = null) is null;

    public string Serialize(object value)
    {
        return value.ToString();
    }

    static T Parse<T>(string v) where T : IParsable<T>
        => T.Parse(v, null);
}
