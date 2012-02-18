using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using XtraSubReport.Winforms.Prototypes;
using XtraSubReport.Winforms.Support;

namespace XtraSubReports.Winforms.Tests.Unit
{
    [TestFixture]
    public class Project_bootstrapper_tests
    {

        [Test]
        public void Should_run_bat_file_if_exists()
        {
            var path = GetNewEmptyPathThatExists();
            const string reportsName = "Reports";
            const string datasourcesName = "Datasources";
            const string actionsName = "Actions";

            var bs = new ProjectBootStrapper(path, reportsName, datasourcesName, actionsName, new Cloner(), new DllLoader());

            File.WriteAllLines(Path.Combine(path,"bootstrapper.bat"), new []{"md findit"});

            File.Exists(Path.Combine(path, "bootstrapper.bat")).Should().BeTrue("batch file needs to exist");


            bs.ExecuteProjectBootStrapperFile("bootstrapper.bat");

            Directory.Exists(Path.Combine(path, "findit")).Should().BeTrue("Folder was created by batch file");



        }

/*        [Test]
        public void Should_auto_create_folders()
        {
            var path = GetNewEmptyPathThatExists();
            const string reportsName = "Reports";
            const string datasourcesName = "Datasources";
            const string actionsName = "Actions";

            var bs = new ProjectBootStrapper(path, reportsName,datasourcesName,actionsName, new Cloner(), new DllLoader());

            bs.CreateFoldersIfNeeded();

            Directory.Exists(Path.Combine(path, reportsName)).Should().BeTrue();
            Directory.Exists(Path.Combine(path, datasourcesName)).Should().BeTrue();
            Directory.Exists(Path.Combine(path, actionsName)).Should().BeTrue();
        }*/


        private static string GetNewEmptyPathThatExists()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);
            return tempPath;
        }

    }
}