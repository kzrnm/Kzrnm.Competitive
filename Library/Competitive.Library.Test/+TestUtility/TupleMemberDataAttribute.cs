using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Kzrnm.Competitive.Testing
{
    internal class TupleMemberDataAttribute : MemberDataAttributeBase
    {
        public TupleMemberDataAttribute(string memberName) : base(memberName, Array.Empty<object>()) { }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return base.GetData(testMethod);
        }

        protected override object[] ConvertDataItem(MethodInfo testMethod, object item)
        {
            if (item == null)
                return null;

            if (item is not ITuple tuple)
                throw new ArgumentException($"Property {MemberName} on {MemberType ?? testMethod.DeclaringType} yielded an item that is not an ITuple");

            var array = new object[tuple.Length];
            for (int i = 0; i < tuple.Length; i++)
            {
                array[i] = tuple[i];
            }
            return array;
        }
    }
}
