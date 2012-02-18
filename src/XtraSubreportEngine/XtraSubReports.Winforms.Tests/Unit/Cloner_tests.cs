using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using XtraSubReport.Winforms.Prototypes;

namespace XtraSubReports.Winforms.Tests.Unit
{
    [TestFixture]
    public class Cloner_tests
    {
        [Test]
        public void Should_copy_file()
        {
            var myDir = CreateTempDir();
            var myDir2 = CreateTempDir();

            using (var fs = File.Create(myDir + "\\" + "Hi.Txt"))
            {
                fs.Flush();
                fs.Close();
            }

            var cloner = new Cloner();
            cloner.Clone(myDir, myDir2);
            File.Exists(myDir2 + "\\" + "Hi.Txt").Should().BeTrue();
        }

        [Test]
        public void Should_copy_file_in_subdirectory()
        {
            var myDir = CreateTempDir();
            var myDir2 = CreateTempDir();

            Directory.CreateDirectory(myDir + "\\ExtraDirectory");

            using (var fs = File.Create(myDir + "\\" + "ExtraDirectory" + "\\Hi.Txt"))
            {
                fs.Flush();
                fs.Close();
            }

            var cloner = new Cloner();
            cloner.Clone(myDir, myDir2);
            File.Exists(myDir2 + "\\" + "ExtraDirectory" + "\\Hi.Txt").Should().BeTrue();
        }


        private static string CreateTempDir()
        {
            

            var myPath = Path.GetTempPath();

            var directoryName = Path.Combine(myPath, Guid.NewGuid().ToString());

            Directory.CreateDirectory(directoryName);
            return directoryName;
        }
    }
}