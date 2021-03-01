using System;
using System.IO;
using Chartix.Infrastructure.Interfaces;

namespace Chartix.Infrastructure.Services
{
    public class FileReaderService : IDisposable
    {
        private readonly IFileService _fileService;
        private bool _fileCreated;
        private FileStream _readStream;

        public FileReaderService(IFileService fileService)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        public byte[] CreateFile()
        {
            if (!Exists())
            {
                _fileService.CreateLocalFile();
                _fileCreated = true;
            }

            return File.ReadAllBytes(_fileService.Filename);
        }

        public FileStream GetFileStream()
        {
            if (!Exists())
            {
                _fileService.CreateLocalFile();
                _fileCreated = true;
            }

            return _readStream = File.OpenRead(_fileService.Filename);
        }

        public bool DeleteFile()
        {
            CloseStream();

            if (Exists())
            {
                try
                {
                    File.Delete(_fileService.Filename);
                    _fileCreated = false;
                    return true;
                }
                catch (Exception)
                {
                    // sorry we couldn't delete
                }
            }

            return false;
        }

        public void Dispose()
        {
            CloseStream();
            DeleteFile();
        }

        private bool Exists() => _fileCreated && File.Exists(_fileService.Filename);

        private void CloseStream() => _readStream?.Close();
    }
}
