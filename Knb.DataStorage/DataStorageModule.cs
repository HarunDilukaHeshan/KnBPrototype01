using Knb.DataStorage.Options;
using Knb.DataStorage.PTurnDataStorage;
using Knb.DataStorage.Shared;
using Knb.DataStorage.Shared.PTurnDataStorage;
using Knb.DataStorage.Shared.XmlStorage;
using Knb.DataStorage.Storage;
using Knb.DataStorage.Storage.MLModelFilesStorage;
using Knb.DataStorage.Storage.TextFilesStorage;
using Knb.DataStorage.XmlStorage;
using Knb.ServiceContainer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Knb.DataStorage
{
    [DependsOn(
        typeof(DataStorageSharedModule))]
    public class DataStorageModule : KnbModuleBase
    {
        public DataStorageModule(IConfiguration configuration)
            : base(configuration)
        { }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DataStorageOptions>(Configuration.GetSection(DataStorageOptions.Position));
            services.Configure<XmlDataFileNodeNames>(Configuration.GetSection(XmlDataFileNodeNames.Position));
            services.Configure<XmlDataFileSchemaInfo>(Configuration.GetSection(XmlDataFileSchemaInfo.Position));
            services.Configure<FilesIoOptions>(Configuration.GetSection(FilesIoOptions.Position));
            services.Configure<DataFileChunkOptions>(Configuration.GetSection(DataFileChunkOptions.Position));
            services.Configure<ProcessedDataFileChunkOptions>(Configuration.GetSection(ProcessedDataFileChunkOptions.Position));
            services.Configure<PTurnDataFileChunkOptions>(Configuration.GetSection(PTurnDataFileChunkOptions.Position));
            services.Configure<ProcessedDataFileStructure>(Configuration.GetSection(ProcessedDataFileStructure.Position));

            services.AddSingleton<IDataFilesStorage>(provider =>
            {
                var options = provider.GetService<IOptions<DataStorageOptions>>().Value ?? throw new InvalidOperationException();
                var filesIoOptions = provider.GetService<IOptions<FilesIoOptions>>().Value ?? throw new InvalidOperationException();
                var chunksOptions = provider.GetService<IOptions<DataFileChunkOptions>>().Value ?? throw new InvalidOperationException();

                return new DataFilesStorage(options.DataFilesStorageSection, filesIoOptions, chunksOptions);
            });

            services.AddSingleton<IProcessedDataFilesStorage>(provider =>
            {
                var options = provider.GetService<IOptions<DataStorageOptions>>().Value ?? throw new InvalidOperationException();
                var filesIoOptions = provider.GetService<IOptions<FilesIoOptions>>().Value ?? throw new InvalidOperationException();
                var chunksOptions = provider.GetService<IOptions<ProcessedDataFileChunkOptions>>().Value ?? throw new InvalidOperationException();

                return new ProcessedDataFilesStorage(options.ProcessedDataFilesStorageSection, filesIoOptions, chunksOptions);
            });

            services.AddSingleton<IMLModelFilesStorage>(provider =>
            {
                var options = provider.GetService<IOptions<DataStorageOptions>>().Value ?? throw new InvalidOperationException();
                var filesIoOptions = provider.GetService<IOptions<FilesIoOptions>>().Value ?? throw new InvalidOperationException();

                return new MLModelFilesStorage(options.MLModelFiles, filesIoOptions);
            });

            services.AddSingleton<ITextFilesStorage>(provider =>
            {
                var options = provider.GetService<IOptions<DataStorageOptions>>().Value ?? throw new InvalidOperationException();
                var filesIoOptions = provider.GetService<IOptions<FilesIoOptions>>().Value ?? throw new InvalidOperationException();

                return new TextFilesStorage(options.TextFiles, filesIoOptions);
            });

            services.AddSingleton<IXmlDataFilesStorage, XmlDataFilesStorage>();
            services.AddSingleton<IPTurnDataFilesStorage, PTurnDataFilesStorage>();
            services.AddSingleton<IMetricsDataStorage, MetricsDataStorage.MetricsDataStorage>();
        }        
    }
}
