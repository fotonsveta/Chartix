namespace Chartix.Infrastructure.Interfaces
{
    public interface IFileService
    {
        public string Filename { get; }

        public void CreateLocalFile();
    }
}
