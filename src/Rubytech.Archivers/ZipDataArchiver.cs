using Rubytech.Abstractions;
using Rubytech.Archivers.Interfaces;
using Rubytech.Json.SerializationOptions;
using Rubytech.TimeProviders.Interfaces;
using System.IO.Compression;
using System.Text.Json;

namespace Rubytech.Archivers
{
    /// <summary>
    /// Архиватор данных в Zip.
    /// </summary>
    public class ZipDataArchiver : BaseDisposable, IDataArchiver
    {
        private readonly FileStream _stream;
        private readonly ZipArchive _archive;

        private const string _fileExtensions = "zip";

        /// <summary>
        /// Инициализация архиватора данных.
        /// </summary>
        /// <param name="directoryPath">Папка, в которой будет сохранен архив.</param>
        /// <param name="timeProvider">Провайдер времени, использующийся для названия архива.</param>
        public ZipDataArchiver(string directoryPath, ITimeProvider timeProvider)
        {
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
            // Создаем пустой файл внутри архива.
            ZipArchiveEntry entry = _archive.CreateEntry(fileName);

            // Открываем стрим для записи json и сериализуем данные в него.
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