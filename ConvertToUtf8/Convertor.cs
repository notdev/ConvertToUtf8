﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConvertToUtf8.SimpleHelpers;

namespace ConvertToUtf8
{
    public class Convertor
    {
        public IEnumerable<string> GetSrtFiles(string folderPath)
        {
            var files = Directory.EnumerateFiles(folderPath, "*.srt", SearchOption.AllDirectories);
            return files;
        }

        /// <summary>
        ///     Converts all files in a folder and all of its subfolders
        /// </summary>
        /// <param name="folderPath"></param>
        public void ConvertAllFiles(string folderPath)
        {
            var files = GetSrtFiles(folderPath).ToList();

            if (!files.Any())
            {
                Console.WriteLine($"No .srt files found in {folderPath}");
                return;
            }

            Console.WriteLine($"Found {files.Count} .srt files. Converting them now.");
            Parallel.ForEach(files, ConvertFile);
            Console.WriteLine("Converting done");
        }

        private void BackupFile(string sourceFile)
        {
            var targetFile = Path.ChangeExtension(sourceFile, ".bk");

            try
            {
                File.Copy(sourceFile, targetFile);
            }
            catch (IOException ex)
            {
                // No need to create backup if it already exists
                if (ex.Message.Contains("already exists"))
                {
                    return;
                }
                throw;
            }
        }

        internal void ConvertFile(string pathToFile)
        {
            Console.WriteLine(Path.GetFileName(pathToFile));
            BackupFile(pathToFile);
            var encoding = FileEncoding.DetectFileEncoding(pathToFile);
            if (Equals(encoding, Encoding.UTF8))
            {
                Console.WriteLine($"Skipping {pathToFile}, it is already UTF8");
                return;
            }

            var originalContent = File.ReadAllText(pathToFile, encoding);
            File.WriteAllText(pathToFile, originalContent, Encoding.UTF8);
        }
    }
}