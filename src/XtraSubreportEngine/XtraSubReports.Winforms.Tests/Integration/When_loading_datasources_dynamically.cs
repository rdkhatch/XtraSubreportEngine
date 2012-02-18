using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using FluentAssertions;

namespace XtraSubReports.Winforms.Tests.Integration
{
    /*public class When_loading_datasources_dynamically : SpecificationBase
    {


        



        private List<string> _assemblyFilePaths;

        public override void Given()
        {
            var jsonRelativePath = "JSON";
            var binFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var jsonAbsolutePath = Path.Combine(binFolder, jsonRelativePath);

            _assemblyFilePaths = Directory.EnumerateFiles(jsonAbsolutePath, "*.dll").ToList();
        }

        public override void When()
        {
            // Loading assemblies dynamically - using LoadFrom()
            foreach (var assemblyFilePath in _assemblyFilePaths)
                Assembly.LoadFrom(assemblyFilePath);
        }

        [Then]
        public void should_have_DBS_Datasouce_ReadModel_assembly()
        {
            var assembly = GetDBSDataSourceReadModelAssembly();
            assembly.Should().NotBeNull();
        }

        [Then]
        public void should_be_able_to_create_report_read_model()
        {
            var assembly = GetDBSDataSourceReadModelAssembly();
            var type = assembly.GetTypes().Single(o => o.Name.Contains("CoveragePeriod"));
            var instance = Activator.CreateInstance(type);
            instance.Should().NotBeNull();
        }

        [Then]
        public void should_have_newtonsoft_assembly_to_deserialize_JSON()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().Single(o => o.FullName.Contains("Newtonsoft"));
            assembly.Should().NotBeNull();
        }

        [Then]
        public void should_have_DBS_DesignTimeDatasource_assembly()
        {
            var assembly = GetDBSDesignTimeDataSourceAssembly();
            assembly.Should().NotBeNull();
        }

        [Then]
        public void should_have_DataSource_provider_type()
        {
            var assembly = GetDBSDesignTimeDataSourceAssembly();

            var providerType = assembly.GetTypes().First(o => typeof(IReportDatasourceProvider).IsAssignableFrom(o));

            bool isProvider = typeof(IReportDatasourceProvider).IsAssignableFrom(providerType);
            isProvider.Should().BeTrue();
        }

        [Then]
        public void should_provide_its_path()
        {
            var assembly = GetDBSDesignTimeDataSourceAssembly();
            assembly.Location.Should().Contain("JSON");
        }

        [Then]
        public void should_resolve_using_autofac()
        {
            var provider = ResolveDataSourceProvider();
            provider.Should().NotBeNull();
        }

        [Then]
        public void should_return_metadata_for_JSON_data_sources()
        {
            var provider = ResolveDataSourceProvider();
            provider.GetReportDatasources().Count().Should().BeGreaterThan(0);
        }

        [Then]
        public void should_deserialize_all_datasources_successfully()
        {
            var provider = ResolveDataSourceProvider();
            var metadatas = provider.GetReportDatasources();

            foreach (var metadata in metadatas)
            {
                object datasource = provider.GetReportDatasource(metadata.UniqueId);
                datasource.Should().NotBeNull();
            }
        }


        private IReportDatasourceProvider ResolveDataSourceProvider()
        {
            var assembly = GetDBSDesignTimeDataSourceAssembly();

            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces();
            var container = builder.Build();
            var provider = container.Resolve<IReportDatasourceProvider>();

            return provider;
        }



        private Assembly GetDBSDesignTimeDataSourceAssembly()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().Single(o => o.FullName.Contains("DBS.PlanDesigner.Reports.DesignTime"));
            return assembly;
        }

        private Assembly GetDBSDataSourceReadModelAssembly()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().Single(o => o.FullName.Contains("DBS.PlanDesigner.Reports.DataSources"));
            return assembly;
        }
    }*/
}