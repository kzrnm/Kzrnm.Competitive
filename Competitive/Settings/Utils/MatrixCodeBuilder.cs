using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Competitive.Settings.Utils
{
    public static class MatrixCodeBuilder
    {
        public static string Create(int size)
        {
            var name = $"Matrix{size}x{size}";
            var sb = new StringBuilder();
            sb.AppendLine($@"using System;
using AtCoder.Operators;
using 凾 = System.Runtime.CompilerServices.MethodImplAttribute;
namespace Kzrnm.Competitive
{{
    public readonly struct {name}<T, TOp>
        where TOp : struct, IArithmeticOperator<T>
    {{
        private static TOp op = default;");
            {
                var rowTuple = "(" + FormatJoin("T C{0}") + ")";
                for (int i = 0; i < size; i++)
                    sb.AppendLine($"        public readonly {rowTuple} R{i};");
                var constructorArgs = FormatJoin($"{rowTuple} row{{0}}");
                sb.AppendLine($"public {name}({string.Join(", ", constructorArgs)})");
                sb.AppendLine("        {");
                for (int i = 0; i < size; i++)
                    sb.AppendLine($"            this.R{i} = row{i};");
                sb.AppendLine("        }");
            }
            {
                //Identity
                sb.AppendLine($"        public static readonly {name}<T, TOp> Identity = new {name}<T, TOp>(");
                for (int i = 0; i < size; i++)
                {
                    sb.Append("            (");
                    for (int j = 0; j < size; j++)
                    {
                        if (i == j)
                            sb.Append("op.MultiplyIdentity");
                        else
                            sb.Append("default(T)");
                        if (j + 1 < size)
                            sb.Append(", ");
                    }
                    if (i + 1 < size)
                        sb.AppendLine("),");
                    else
                        sb.AppendLine("));");
                }
                sb.AppendLine();
            }

            {
                // 単項マイナス
                sb.AppendLine($"        public static {name}<T, TOp> operator -({name}<T, TOp> x) => new {name}<T, TOp>(");
                for (int i = 0; i < size; i++)
                {
                    sb.Append("            (");
                    for (int j = 0; j < size; j++)
                    {
                        sb.Append($"op.Minus(x.R{i}.C{j})");
                        if (j + 1 < size)
                            sb.Append(", ");
                    }
                    if (i + 1 < size)
                        sb.AppendLine("),");
                    else
                        sb.AppendLine("));");
                }
                sb.AppendLine();
            }
            {
                // 加算
                sb.AppendLine($"        public static {name}<T, TOp> operator +({name}<T, TOp> x, {name}<T, TOp> y) => new {name}<T, TOp>(");
                for (int i = 0; i < size; i++)
                {
                    sb.Append("            (");
                    for (int j = 0; j < size; j++)
                    {
                        sb.Append($"op.Add(x.R{i}.C{j}, y.R{i}.C{j})");
                        if (j + 1 < size)
                            sb.Append(", ");
                    }
                    if (i + 1 < size)
                        sb.AppendLine("),");
                    else
                        sb.AppendLine("));");
                }
                sb.AppendLine();
            }
            {
                // 減算
                sb.AppendLine($"        public static {name}<T, TOp> operator -({name}<T, TOp> x, {name}<T, TOp> y) => new {name}<T, TOp>(");
                for (int i = 0; i < size; i++)
                {
                    sb.Append("            (");
                    for (int j = 0; j < size; j++)
                    {
                        sb.Append($"op.Subtract(x.R{i}.C{j}, y.R{i}.C{j})");
                        if (j + 1 < size)
                            sb.Append(", ");
                    }
                    if (i + 1 < size)
                        sb.AppendLine("),");
                    else
                        sb.AppendLine("));");
                }
                sb.AppendLine();
            }
            {
                // 乗算
                sb.AppendLine($"        public static {name}<T, TOp> operator *({name}<T, TOp> x, {name}<T, TOp> y) => new {name}<T, TOp>(");

                for (int i = 0; i < size; i++)
                {
                    sb.AppendLine("            (");
                    for (int j = 0; j < size; j++)
                    {
                        for (int k = 1; k < size; k++)
                            sb.Append("op.Add(");
                        for (int k = 0; k < size; k++)
                        {
                            sb.Append($"op.Multiply(x.R{i}.C{k}, y.R{k}.C{j})");
                            if (k > 0)
                                sb.Append(')');
                            sb.Append(',');
                        }
                        if (j + 1 == size)
                            sb.Remove(sb.Length - 1, 1);
                        sb.AppendLine();
                    }
                    if (i + 1 < size)
                        sb.AppendLine("),");
                    else
                        sb.AppendLine("));");
                }
                sb.AppendLine();
            }

            {
                // スカラー倍
                sb.AppendLine($"        public static {name}<T, TOp> operator *(T a, {name}<T, TOp> y) => new {name}<T, TOp>(");
                for (int i = 0; i < size; i++)
                {
                    sb.Append("            (");
                    for (int j = 0; j < size; j++)
                    {
                        sb.Append($"op.Multiply(a, y.R{i}.C{j})");
                        if (j + 1 < size)
                            sb.Append(", ");
                    }
                    if (i + 1 < size)
                        sb.AppendLine("),");
                    else
                        sb.AppendLine("));");
                }
                sb.AppendLine();
            }
            {
                // ベクトル乗算
                var vectorResult = "(" + FormatJoin("T v{0}") + ")";
                sb.AppendLine("        /// <summary>");
                sb.AppendLine("        /// 2次元ベクトルにかける");
                sb.AppendLine("        /// </summary>");
                sb.AppendLine($"        public static {vectorResult} operator *({name}<T, TOp> mat, {vectorResult} vector) => mat.Multiply(vector);");
                sb.AppendLine();
                sb.AppendLine("        /// <summary>");
                sb.AppendLine("        /// 2次元ベクトルにかける");
                sb.AppendLine("        /// </summary>");
                sb.AppendLine($"        public {vectorResult} Multiply({vectorResult} vector) => Multiply({FormatJoin("vector.v{0}")});");
                sb.AppendLine();
                sb.AppendLine("        /// <summary>");
                sb.AppendLine("        /// 2次元ベクトルにかける");
                sb.AppendLine("        /// </summary>");
                sb.AppendLine($"        public {vectorResult} Multiply{vectorResult}");
                sb.AppendLine("            => (");

                for (int i = 0; i < size; i++)
                {
                    for (int k = 1; k < size; k++)
                        sb.Append("op.Add(");
                    for (int k = 0; k < size; k++)
                    {
                        sb.Append($"op.Multiply(R{i}.C{k}, v{k})");
                        if (k > 0)
                            sb.Append(')');
                        if (k + 1 < size)
                            sb.Append(',');
                    }
                    if (i + 1 < size)
                        sb.AppendLine(",");
                    else
                    {
                        sb.AppendLine();
                    }
                }
                sb.AppendLine("               );");
            }
            sb.AppendLine($@"

        /// <summary>
        /// <paramref name=""y""/> 乗した行列を返す。
        /// </summary>
        public {name}<T, TOp> Pow(long y) => MathLibGeneric.Pow < {name}<T, TOp>, {name}Operator<T, TOp>>(this, y);
    }}
    public struct {name}Operator<T, TOp> : IArithmeticOperator<{name}<T, TOp>>
        where TOp : struct, IArithmeticOperator<T>
    {{
        public {name}<T, TOp> MultiplyIdentity => {name}<T, TOp>.Identity;

        [凾(256)]
        public {name}<T, TOp> Add({name}<T, TOp> x, {name}<T, TOp> y) => x + y;
        [凾(256)]
        public {name}<T, TOp> Subtract({name}<T, TOp> x, {name}<T, TOp> y) => x - y;
        [凾(256)]
        public {name}<T, TOp> Multiply({name}<T, TOp> x, {name}<T, TOp> y) => x * y;
        [凾(256)]
        public {name}<T, TOp> Minus({name}<T, TOp> x) => -x;

        [凾(256)]
        public {name}<T, TOp> Increment({name}<T, TOp> x) => throw new NotSupportedException();
        [凾(256)]
        public {name}<T, TOp> Decrement({name}<T, TOp> x) => throw new NotSupportedException();
        [凾(256)]
        public {name}<T, TOp> Divide({name}<T, TOp> x, {name}<T, TOp> y) => throw new NotSupportedException();
        [凾(256)]
        public {name}<T, TOp> Modulo({name}<T, TOp> x, {name}<T, TOp> y) => throw new NotSupportedException();
    }}
}}");
            return sb.ToString();

            string FormatJoin(string format) => string.Join(", ", Enumerable.Range(0, size).Select(n => string.Format(format, n)));
        }
    }
}
