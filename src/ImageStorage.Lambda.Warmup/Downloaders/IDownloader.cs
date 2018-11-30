namespace ImageStorage.Lambda.Warmup
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public interface IDownloader
    {
        Task<Stream> DownloadAsync(Uri url);

        void Dispose();
    }
}