using System;
using System.Linq;
using System.Reflection;

internal class Program
{
    static void Main(string[] args)
    {
        var name = args.Length > 0 ? args[0] : Console.ReadLine();
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();
        Type type;
        while (true)
        {
            var matches = types.Where(t => t.Name.StartsWith(name)).ToArray();
            if (matches.Length == 0)
                Console.WriteLine($"{name} から始まるクラスが見つかりません");
            else if (matches.Length == 1)
            {
                type = matches[0];
                break;
            }
            else
            {
                Console.WriteLine($"{name} から始まるクラスが複数見つかりました");
                Console.WriteLine(string.Join(", ", matches.Select(t => t.Name)));
            }
            name = Console.ReadLine();
        }

        type.GetMethod("Main", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).Invoke(type, null);
    }
}
