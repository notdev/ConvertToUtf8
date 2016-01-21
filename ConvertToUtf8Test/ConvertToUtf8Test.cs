using System;
using System.IO;
using System.Linq;
using ConvertToUtf8;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConvertToUtf8Test
{
    [TestClass]
    public class ConvertToUtf8Test
    {
        private readonly Convertor convertor = new Convertor();
        private readonly string testDirectory = "TestDirectory";
        private readonly string testFile = "ExampleANSISubtitle.srt";

        [TestMethod]
        public void GetSrtFiles_ReturnsAllSubtitleFiles()
        {
            var files = convertor.GetSrtFiles(testDirectory);
            Assert.IsTrue(files.Count() == 1, $"Incorrect number of subtitles found. Expected 1. Returned {files.Count()}");
        }

        [TestMethod]
        public void Convertor_ConvertsFiles()
        {
            var testFolder = testDirectory + "_ConvertFileTest";
            SetupTest(testFolder);

            try
            {
                convertor.ConvertAllFiles(testFolder);
            }
            catch (Exception ex)
            {
                Assert.Fail($"Expected no exceptions during convert. Exception occured: {ex.Message} Stacktracek: {ex.StackTrace}");
            }

            string backupFilePath = testFolder + "\\" +  testFile;
            backupFilePath = Path.ChangeExtension(backupFilePath, ".bk");
            Assert.IsTrue(File.Exists(backupFilePath), "Backup file was not created");

            DeleteDirectory(testFolder);
        }

        private void SetupTest(string tempTestDirectory)
        {
            new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyDirectory(testDirectory, tempTestDirectory, true);
        }

        private void DeleteDirectory(string directoryPath)
        {
            var files = Directory.EnumerateFiles(directoryPath, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                File.Delete(file);
            }
            Directory.Delete(directoryPath, true);
        }
    }
}