using Kzrnm.Competitive;
using Kzrnm.Competitive.Testing.Serializer;
using System;
using System.Linq;
using Xunit.Sdk;

[assembly: RegisterXunitSerializer(typeof(LongMatrixSerializer), [
    typeof(LongMatrix2x2),
    typeof(LongMatrix3x3),
    typeof(LongMatrix4x4),
])]


namespace Kzrnm.Competitive.Testing.Serializer;

public class LongMatrixSerializer : IXunitSerializer
{
    public object Deserialize(Type type, string serializedValue)
    {
        var s = serializedValue.Split('\0').Select(long.Parse).ToArray();
        if (type == typeof(LongMatrix2x2))
            return new LongMatrix2x2(
                (s[0], s[1]),
                (s[2], s[3]));
        if (type == typeof(LongMatrix3x3))
            return new LongMatrix3x3(
                (s[0], s[1], s[2]),
                (s[3], s[4], s[5]),
                (s[6], s[7], s[8]));
        if (type == typeof(LongMatrix4x4))
            return new LongMatrix4x4(
                (s[0], s[1], s[2], s[3]),
                (s[4], s[5], s[6], s[7]),
                (s[8], s[9], s[10], s[11]),
                (s[12], s[13], s[14], s[15]));
        throw new NotSupportedException();
    }

    public bool IsSerializable(Type type, object value, out string failureReason)
        => (failureReason = null) == null;
    public string Serialize(object value)
    {
        var arr = value switch
        {
            LongMatrix2x2 m2 => m2.ToArray().SelectMany(t => t).ToArray(),
            LongMatrix3x3 m3 => m3.ToArray().SelectMany(t => t).ToArray(),
            LongMatrix4x4 m4 => m4.ToArray().SelectMany(t => t).ToArray(),
            _ => throw new NotSupportedException(),
        };
        return string.Join('\0', arr);
    }
}
