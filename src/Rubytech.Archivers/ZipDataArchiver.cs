using Rubytech.Abstractions.BaseObjects;
using Rubytech.Archivers.Interfaces;
using Rubytech.Json.SerializationOptions;
using Rubytech.TimeProviders.Interfaces;
using System.IO.Compression;
using System.Text.Json;

namespace Rubytech.Archivers
{
    public class ZipDataArchiver : BaseDisposable, IDataArchiver
    {
        private readonly FileStream _stream;
        private readonly ZipArchive _archive;

        private const string _fileExtensions = "zip";

        public ZipDataArchiver(string directoryPath, ITimeProvider timeProvider)
        {
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException("Директория не существует.");
            }

            string fileName = string.Join(
                '.', 
                timeProvider.GetCurrentDateTimeInISO8601(), 
                _fileExtensions);
            
            _stream = new FileStream(Path.Combine(directoryPath, fileName), FileMode.Create);
            _archive = new ZipArchive(_stream, ZipArchiveMode.Create, true);
        }

        public async Task AddDataToEntryAsync<T>(
            T data, 
            string fileName)
        {
            ZipArchiveEntry entry = _archive.CreateEntry(fileName);

            using Stream entryStream = entry.Open();

            await JsonSerializer.SerializeAsync(
                entryStream, 
                data, 
                RubytechWriteSerializationOptions.Value);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _archive?.Dispose();
                _stream?.Dispose();
            }
        }
    }
}