using System;
using System.IO;
using System.Threading;

namespace NpmFolderRenameIssue
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a buffer of some size
            var buf = new byte[1024];
            for (int n = 0; n < buf.Length; n++)
            {
                buf[n] = 66;
            }

            // This file should not exist yet
            // Note that the presence of this existence check is what triggers the bug below!!
            Console.WriteLine("Exists (should be false): " + File.Exists("test/test2/file20"));

            // Create a directory with 20 files
            Directory.CreateDirectory("test/subdir/test");

            for (var i = 1; i <= 20; i++)
            {
                File.WriteAllBytes(String.Format("test/subdir/test/file{0:00}", i), buf);
            }

            // Rename the directory
            Directory.Move("test/subdir/test", "test/test2");

            var start = DateTime.UtcNow;

            // List the files. Here, they all show up
            foreach (var path in Directory.GetFiles("test/test2"))
            {
                Console.WriteLine(path);
            }

            for (; ; )
            {
                // This should be true, but when running on a (cross machine) UNC share, it takes almost 5 seconds to become true!
                if (File.Exists("test/test2/file20")) break;

                Console.WriteLine("After {0} milliseconds, test/test2/file20 doesn't show as existing", (DateTime.UtcNow - start).TotalMilliseconds);
                Thread.Sleep(100);
            }

            Console.WriteLine("After {0} milliseconds, test/test2/file20 correctly shows as existing!", (DateTime.UtcNow - start).TotalMilliseconds);
            Console.ReadLine();
        }
    }
}
