using Knb.DataStorage.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Knb.DataStorage.Storage
{
    public abstract class DataStorageBase : IDataStorage
    {
        protected StorageSectionDetails SectionDetails { get; }
        protected FilesIoOptions FilesIoOptions { get; }
        public DataStorageBase(
            StorageSectionDetails sectionDetails, 
            FilesIoOptions filesIoOptions)
        {
            SectionDetails = sectionDetails ?? throw new ArgumentNullException();
            FilesIoOptions = filesIoOptions ?? throw new ArgumentNullException();
        }

        public abstract IAsyncEnumerable<byte[]> GetAsyncChunks(string fileName);

        public async Task ClearFilesAsync()
        {
            await Task.Run(() =>
            {
                var files = Directory.GetFiles(SectionDetails.Path.ToString());

                foreach (var file in files)
                    File.Delete(file);
            });
        }

        public virtual async Task<bool> CreateFileAsync(string fileName, byte[] buffer)
        {
            fileName = string.IsNullOrWhiteSpace(fileName) ? fileName : Path.GetFileName(fileName);

            if (string.IsNullOrWhiteSpace(fileName) || !IsValidFileName(fileName)
                || buffer == null || buffer.Length == 0) throw new ArgumentException();

            var datPath = GetPath(fileName) ?? throw new ArgumentException();

            if (File.Exists(datPath)) throw new InvalidOperationException("File already exists");

            if (!Directory.Exists(SectionDetails.Path.ToString()))
                Directory.CreateDirectory(SectionDetails.Path.ToString());

            using var writer = File.Create(datPath, FilesIoOptions.BufferSize);
            await writer.WriteAsync(buffer);
            await writer.FlushAsync();
            await writer.DisposeAsync();

            return File.Exists(datPath);
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName) || IsValidFileName(fileName))
                throw new ArgumentException();

            var datPath = GetPath(fileName) ?? throw new ArgumentException();

            await Task.Run(() => { File.Delete(datPath); });

            return !File.Exists(datPath);
        }

        public async Task<FileInfo[]> GetFilesInfo()
        {
            if (!Directory.Exists(SectionDetails.Path.ToString())) Directory.CreateDirectory(SectionDetails.Path.ToString());

            var files = Directory.GetFiles(SectionDetails.Path.ToString());
            var fileInfoList = new List<FileInfo>();

            await Task.Run(() =>
            {
                foreach (var file in files)
                {
                    using var st = File.OpenRead(file);
                    var len = st.Length;
                    var dr = SectionDetails.Path.ToString();
                    var ext = Path.GetExtension(file);
                    st.Close();
                    
                    fileInfoList.Add(new FileInfo(Path.GetFileName(file), ext, dr, len));
                }
            });

            return fileInfoList.ToArray();
        }

        public async Task<FileInfo> GetFileInfo(string fileName)
        {
            FileInfo fileInfo = null;

            await Task.Run(() => {
                var path = GetPath(fileName) ?? throw new InvalidOperationException("Invalid file name");

                using var st = File.OpenRead(path);
                var len = st.Length;
                var dr = SectionDetails.Path.ToString();
                var ext = Path.GetExtension(path);
                st.Close();

                fileInfo = new FileInfo(fileName, dr, ext, len);
            });

            return fileInfo;
        }

        public bool Exists(string fileName)
        {
            var path = GetPath(fileName) ?? throw new InvalidOperationException("Invalid file name");
            return File.Exists(path);
        }

        public FileStream GetReadStream(string fileName)
        {
            if (!IsValidFileName(fileName)) throw new ArgumentException();

            var datPath = GetPath(fileName);

            if (!File.Exists(datPath)) throw new FileNotFoundException();            

            return File.OpenRead(datPath);
        }

        public FileStream GetWriteStream(string fileName)
        {
            if (!IsValidFileName(fileName)) throw new ArgumentException();

            var datPath = GetPath(fileName);

            if (!File.Exists(datPath)) throw new FileNotFoundException();

            var writer = File.OpenWrite(datPath);           

            return writer;
        }

        protected virtual bool IsValidFileName(string fileName)
        {
            var fn = Path.GetFileName(fileName);
            var dr = Path.GetDirectoryName(fileName);
            
            var isValid = (Path.GetInvalidFileNameChars().FirstOrDefault(c => fn.IndexOf(c) > -1) == '\0');
            isValid = isValid && (Path.GetInvalidPathChars().FirstOrDefault(c => fn.IndexOf(c) > -1) == '\0');

            return isValid && string.IsNullOrWhiteSpace(dr);
        }

        protected virtual string GetPath(string fileName)
        {
            if (!IsValidFileName(fileName)) return null;

            var path = Path.Combine(SectionDetails.Path.ToString(), fileName);
            return Path.ChangeExtension(path, "dat");
        }
    }    
}
