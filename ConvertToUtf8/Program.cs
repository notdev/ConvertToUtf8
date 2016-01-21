using System;
using System.IO;

namespace ConvertToUtf8
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintUsage();
                return;
            }

            try
            {
                var verifiedPath = VerifyArguments(args);

                var convertor = new Convertor();
                convertor.ConvertAllFiles(verifiedPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("ConvertToUtf8 converts all .srt files in a folder and its subfolders to UTF-8");
            Console.WriteLine("Usage: ConvertToUtf8 <Folder Path>");
        }

        private static string VerifyArguments(string[] args)
        {
            var folderPath = args[0];
            if (!Directory.Exists(folderPath))
            {
                throw new Exception(
                    $"Directory '{folderPath}' does not exist or is not accessible. If it exists, make sure permissions allow read/write.");
            }
            return folderPath;
        }
    }
}