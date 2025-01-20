using Kzrnm.Competitive.Testing.Serializer;
using System;
using System.Collections;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(typeof(BitArraySerializer), [
    typeof(BitArray),
])]


namespace Kzrnm.Competitive.Testing.Serializer;

public class BitArraySerializer : IXunitSerializer
{
    public object Deserialize(Type type, string serializedValue)
    {
        if (type != typeof(BitArray))
            throw new NotSupportedException();
        var b = new bool[serializedValue.Length];
        for (int i = 0; i < b.Length; i++)
        {
            b[i] = serializedValue[i] switch
            {
                '0' => false,
                '1' => true,
                _ => throw new NotSupportedException(),
            };
        }
        return new BitArray(b);
    }

    public bool IsSerializable(Type type, object value, out string failureReason)
        => (failureReason = null) is null;

    public string Serialize(object value)
    {
        var b = (BitArray)value;
        var chrs = new char[b.Length];
        for (int i = 0; i < chrs.Length; i++)
        {
            chrs[i] = b[i] ? '1' : '0';
        }
        return new string(chrs);
    }
}
