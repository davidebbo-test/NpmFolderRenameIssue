using System;
using System.IO;

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

            // Create a directory with 20 files
            Directory.CreateDirectory("test");
            Directory.CreateDirectory("test/subdir");
            Directory.CreateDirectory("test/subdir/test");

            for (var i = 1; i <= 20; i++)
            {
                File.WriteAllBytes("test/subdir/test/file" + i, buf);
            }

            // Rename the directory
            Directory.Move("test/subdir/test", "test/test2");

            // Read the directory
            foreach (var path in Directory.GetFiles("test/test2"))
            {
                Console.WriteLine(path);
            }

            // This file should now be there
            Console.WriteLine(File.Exists("test/test2/file20"));
        }
    }
}
