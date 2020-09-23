using AtCoder;
using AtCoder.Expand;
using System;
using System.IO;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace SourceCodeWatcher
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0 || !File.Exists(args[0]))
            {
                Console.Error.WriteLine("need file path");
                return 1;
            }
            Console.WriteLine("Press 'q' to quit.");
            filepath = Path.GetFullPath(args[0]);
            watcher = GetWatcher();
            Update();

            while (Console.Read() != 'q') ;
            return 0;
        }
        static bool continueOnError = true;

        static string filepath;
        static DateTime lastUpdate = DateTime.MinValue;
        static FileSystemWatcher watcher;

        private static FileSystemWatcher GetWatcher()
        {
            var watcher = new FileSystemWatcher(Path.GetDirectoryName(filepath))
            {
                NotifyFilter = (NotifyFilters)127,
                Filter = "Program.cs*",
            };
            watcher.Created += W_Changed;
            watcher.Deleted += W_Changed;
            watcher.Changed += W_Changed;
            watcher.EnableRaisingEvents = true;
            return watcher;
        }

        private static void W_Changed(object sender, FileSystemEventArgs e) => Update();
        private static async void Update()
        {
            const int maxRetry = 5;
            const int retryWait = 100;
            watcher.EnableRaisingEvents = false;
            try
            {
                int retryCount = 0;
            FileUpdate: try
                {
                    var fileinfo = new FileInfo(filepath);
                    if (lastUpdate < fileinfo.LastWriteTime)
                    {
                        await Task.Delay(retryWait);
                        lastUpdate = fileinfo.LastWriteTime;
                        Console.WriteLine($"start Expanding: {filepath}");
                        Expander.Expand(filepath, expandMethod: ExpandMethod.Strict);
                        Console.WriteLine($"finish Expanding: {filepath}");
                    }
                    else
                    {
                        Console.WriteLine("file not chenged.");
                    }
                }
                catch (IOException e) when (retryCount++ < maxRetry)
                {
                    Console.WriteLine($"Error Expanding: {e.Message}, retry: {retryCount}");
                    await Task.Delay(retryWait);
                    goto FileUpdate;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                if (!continueOnError)
                    throw;
            }
            watcher.EnableRaisingEvents = true;
        }
    }
}
