using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using XtraSubReport.Winforms;

namespace XtraSubReports.Winforms.Tests.Unit
{

    [TestFixture]
    public class ProjectBootstrapper_Tests
    {

        [Test]
        public void Should_run_bat_file_if_exists()
        {
            var path = GetNewEmptyPathThatExists();
            const string reportsName = "Reports";
            const string datasourcesName = "Datasources";
            const string actionsName = "Actions";

            var bs = new ProjectBootStrapper(path, reportsName, datasourcesName, actionsName);

            File.WriteAllLines(Path.Combine(path,"bootstrapper.bat"), new []{"md findit"});

            File.Exists(Path.Combine(path, "bootstrapper.bat")).Should().BeTrue("batch file needs to exist");


            bs.ExecuteBatchFile("bootstrapper.bat");

            Directory.Exists(Path.Combine(path, "findit")).Should().BeTrue("Folder was created by batch file");



        }

        [Test]
        public void Should_auto_create_folders()
        {
            var path = GetNewEmptyPathThatExists();
            const string reportsName = "Reports";
            const string datasourcesName = "Datasources";
            const string actionsName = "Actions";

            var bs = new ProjectBootStrapper(path, reportsName,datasourcesName,actionsName);

            bs.CreateFoldersIfNeeded();

            Directory.Exists(Path.Combine(path, reportsName)).Should().BeTrue();
            Directory.Exists(Path.Combine(path, datasourcesName)).Should().BeTrue();
            Directory.Exists(Path.Combine(path, actionsName)).Should().BeTrue();
        }


        private static string GetNewEmptyPathThatExists()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);
            return tempPath;
        }

    }

    [TestFixture]
    public class AppBootstrapper_Tests
    {
        [Test]
        public void Should_auto_create_folder()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            Directory.Exists(tempPath).Should().BeFalse("Test is in invalid state, cannot proceed");
            var bootStrapper = new AppBootStrapper(tempPath);

            bootStrapper.CreateRootPathIfNeeded();

            Directory.Exists(tempPath).Should().BeTrue("AppBootStrapper did not create a temp path as expected");

        }

        [Test]
        public void Should_detect_when_no_projects_exist()
        {
            //Given that the path already exists;
            var tempPath = GetNewEmptyPathThatExists();
            var bs = new AppBootStrapper(tempPath);
            bs.DetectProjectMode().Should().Be(AppProjectsStructureMode.None);
        }


        [Test]
        public void Should_detect_when_no_path_projects_exist()
        {
            //Given that the path already exists;
            var tempPath = GetNewEmptyPathThatDoesntExists();
            var bs = new AppBootStrapper(tempPath);
            bs.DetectProjectMode().Should().Be(AppProjectsStructureMode.None);
        }



        [Test]
        public void Should_detect_when_multiple_projects_exist_with_correct_count_when_three()
        {
            var tempPath = GetPathWithThreeProjects();
            var bs = new AppBootStrapper(tempPath);

            bs.DetectProjectMode().Should().Be(AppProjectsStructureMode.MultipleUnchosen);
            bs.GetProjects().Count().Should().Be(3);
        }


        [Test]
        public void Should_detect_when_multiple_projects_exist_with_correct_count_when_three_and_chosen()
        {
            var tempPath = GetPathWithThreeProjects();
            var bs = new AppBootStrapper(tempPath);

            bs.SetProjectName("Project1");
            bs.DetectProjectMode().Should().Be(AppProjectsStructureMode.MultipleChosen);
        }

        [Test]
        public void Should_detect_when_multiple_projects_exist_with_correct_count_when_three_and_chosen_but_doesnt_exist()
        {
            var tempPath = GetPathWithThreeProjects();
            var bs = new AppBootStrapper(tempPath);
            Exception exception = null;

            try
            {
                bs.SetProjectName("Project45");
            }
            catch (Exception ex)
            {
                exception = ex;
            }


            exception.Should().NotBeNull();
        }

        private static string GetPathWithThreeProjects()
        {
            var tempPath = GetNewEmptyPathThatExists();
            Directory.CreateDirectory(Path.Combine(tempPath, "Project1"));
            Directory.CreateDirectory(Path.Combine(tempPath, "Project2"));
            Directory.CreateDirectory(Path.Combine(tempPath, "Project3"));
            return tempPath;
        }

        [Test]
        public void Should_detect_when_multiple_projects_exist_with_correct_count_when_one()
        {
            var tempPath = GetNewEmptyPathThatExists();
            Directory.CreateDirectory(Path.Combine(tempPath, "Project1"));
            var bs = new AppBootStrapper(tempPath);

            bs.DetectProjectMode().Should().Be(AppProjectsStructureMode.Single);
            bs.GetProjects().Count().Should().Be(1);
        }


        private string GetNewEmptyPathThatDoesntExists()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            return tempPath;
        }

        private static string GetNewEmptyPathThatExists()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);
            return tempPath;
        }
    }
}
