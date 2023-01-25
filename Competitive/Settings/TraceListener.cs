using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Competitive.Runner
{
    internal class TraceListener : TextWriterTraceListener
    {
        public TraceListener(TextWriter writer) : base(writer) { }
        const ConsoleColor NormalColor = ConsoleColor.DarkGreen;
        public override void Write(string message)
        {
            Console.ForegroundColor = NormalColor;
            base.Write(message);
            Console.ResetColor();
        }
        public override void WriteLine(string message)
        {
            if (message.StartsWith("---") && message.EndsWith("---"))
                Console.BackgroundColor = ConsoleColor.DarkYellow;
            else
                Console.ForegroundColor = NormalColor;
            base.WriteLine(message);
            Console.ResetColor();
        }
    }
}
