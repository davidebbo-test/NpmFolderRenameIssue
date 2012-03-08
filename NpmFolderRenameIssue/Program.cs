using System;
using System.IO;
using System.Threading;

namespace NpmFolderRenameIssue
{
    class Program
    {
        static void Main(string[] args)
        {
            // This file doesn't exist yet
            // Note that the presence of this existence check is what triggers the bug below!!
            Console.WriteLine("Exists (should be false): " + File.Exists("test/test2/myfile"));

            // Create a directory, with a file in it
            Directory.CreateDirectory("test/subdir/test");
            File.WriteAllText("test/subdir/test/myfile", "Hello");

            // Rename the directory
            Directory.Move("test/subdir/test", "test/test2");

            var start = DateTime.UtcNow;

            // List the files at the new location. Here, our file shows up fine
            foreach (var path in Directory.GetFiles("test/test2"))
            {
                Console.WriteLine(path);
            }

            for (; ; )
            {
                // Now do a simple existence test. It should also be true, but when running on a (cross machine) UNC share,
                // it takes almost 5 seconds to become true!
                if (File.Exists("test/test2/myfile")) break;

                Console.WriteLine("After {0} milliseconds, test/test2/myfile doesn't show as existing", (DateTime.UtcNow - start).TotalMilliseconds);
                Thread.Sleep(100);
            }

            Console.WriteLine("After {0} milliseconds, test/test2/myfile correctly shows as existing!", (DateTime.UtcNow - start).TotalMilliseconds);
            Console.ReadLine();
        }
    }
}
