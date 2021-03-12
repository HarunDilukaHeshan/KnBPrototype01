using Knb.DataStorage.Options;
using Knb.DataStorage.Shared.XmlStorage;
using Knb.DataStorage.Storage;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace Knb.DataStorage.XmlStorage
{
    public class XmlDataFilesStorage : IXmlDataFilesStorage
    {
        protected IDataFilesStorage DataFilesStorage { get; }
        protected XmlDataFileNodeNames XmlDataFileNodeNames { get; }
        protected XmlDataFileSchemaInfo XmlDataFileSchemaInfo { get; }
        protected FilesIoOptions FilesIoOptions { get; }
        public XmlDataFilesStorage(
            IDataFilesStorage dataFilesStorage,
            IOptions<XmlDataFileNodeNames> xmlDataFileNodeNames,
            IOptions<XmlDataFileSchemaInfo> xmlDataFileSchemaInfo, 
            IOptions<FilesIoOptions> ioOptions)
        {
            if (dataFilesStorage == null || xmlDataFileNodeNames.Value == null ||
                xmlDataFileSchemaInfo.Value == null || ioOptions.Value == null) throw new ArgumentNullException();

            DataFilesStorage = dataFilesStorage;
            XmlDataFileNodeNames = xmlDataFileNodeNames.Value;
            XmlDataFileSchemaInfo = xmlDataFileSchemaInfo.Value;
            FilesIoOptions = ioOptions.Value;
        }

        public async Task<bool> CreateDataFileAsync(string fileName)
        {
            var xmlDoc = new XmlDocument();
            var rootElm = xmlDoc.CreateElement(XmlDataFileNodeNames.Root);

            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", ""));
            xmlDoc.AppendChild(rootElm);

            xmlDoc.DocumentElement.SetAttribute("xmlns", XmlDataFileSchemaInfo.TargetNamespace);
            xmlDoc.DocumentElement.SetAttribute("xmlns:xs", XmlDataFileSchemaInfo.XmlSchemaInstance);
            xmlDoc.DocumentElement.SetAttribute("schemaLocation", XmlDataFileSchemaInfo.XmlSchemaInstance,
                string.Concat(XmlDataFileSchemaInfo.TargetNamespace, " ", XmlDataFileSchemaInfo.XmlSchemaFileUrl));

            rootElm.IsEmpty = false;

            var result = await DataFilesStorage.CreateFileAsync(fileName, Encoding.UTF8.GetBytes(xmlDoc.OuterXml));

            return result;
        }

        public async Task<bool> DeleteDataFileAsync(string fileName)
        {
            return await DataFilesStorage.DeleteFileAsync(fileName);
        }

        public IAsyncEnumerable<XmlDocument> GetAsyncChunks(string fileName)
        {
            return new XmlDataFilesAsyncEnum(() =>
            {
                var readStream = DataFilesStorage.GetReadStream(fileName);
                return XmlReader.Create(readStream, GetReaderSettings());
            });
        }

        public async Task WriteXmlAsync(string fileName, string xmlStr)
        {
            if (!DataFilesStorage.Exists(fileName)) await CreateDataFileAsync(fileName);
          
            var cusXmlDoc = new XmlDocument();

            cusXmlDoc.LoadXml(xmlStr);

            using var ws = DataFilesStorage.GetWriteStream(fileName);
            using var bufferedSt = new BufferedStream(ws, FilesIoOptions.BufferSize);

            var cTag = string.Concat("</", XmlDataFileNodeNames.Root, ">");
            var buffer = Encoding.UTF8.GetBytes(cusXmlDoc.DocumentElement.InnerXml);

            bufferedSt.Position = ws.Length - cTag.Length;
            await bufferedSt.WriteAsync(buffer);
            await bufferedSt.WriteAsync(Encoding.UTF8.GetBytes(cTag));
        }

        public async Task<bool> ValidateXmlFile(string fileName)
        {
            var settings = GetReaderSettings();
            using var st = DataFilesStorage.GetReadStream(fileName);
            using var reader = XmlReader.Create(st, settings);
            var result = true;
            try
            {
                while (await reader.ReadAsync()) { }
            }
            catch(Exception ex)
            {
                result = false;
            }

            reader.Dispose();
            await st.DisposeAsync();

            return result;
        }

        public bool Exists(string fileName)
        {
            return DataFilesStorage.Exists(fileName);
        }
        public async Task<Storage.FileInfo> GetFileInfoAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException();
            return await DataFilesStorage.GetFileInfo(fileName) ?? throw new InvalidOperationException();
        }

        public async Task<Storage.FileInfo[]> GetFilesInfoAsync()
        {
            return await DataFilesStorage.GetFilesInfo();
        }

        private XmlWriterSettings GetWriterSettings()
        {
            var settings = new XmlWriterSettings
            {
                Async = true,
                ConformanceLevel = ConformanceLevel.Document,
                Encoding = Encoding.UTF8
            };

            return settings;
        }

        private XmlReaderSettings GetReaderSettings()
        {
            var settings = new XmlReaderSettings
            {
                Async = true,
                IgnoreWhitespace = true,
                IgnoreComments = true,
                ConformanceLevel = ConformanceLevel.Document,
                ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings | XmlSchemaValidationFlags.ProcessIdentityConstraints,
                ValidationType = ValidationType.Schema
            };

            settings.Schemas.Add(XmlDataFileSchemaInfo.TargetNamespace, XmlDataFileSchemaInfo.XmlSchemaFileUrl);

            return settings;
        }        
    }
}
