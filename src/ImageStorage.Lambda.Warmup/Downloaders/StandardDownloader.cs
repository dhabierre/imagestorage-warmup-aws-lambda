namespace ImageStorage.Lambda.Warmup
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class StandardDownloader : IDownloader, IDisposable
    {
        private readonly HttpClient client;

        public StandardDownloader(Func<HttpClient> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            this.client = builder();
        }

        public async Task<Stream> DownloadAsync(Uri url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            var request = this.Build(url);
            var response = await this.client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync();
        }

        public void Dispose()
        {
            this.client.Dispose();
        }

        protected virtual HttpRequestMessage Build(Uri url)
        {
            if (url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            return new HttpRequestMessage(HttpMethod.Get, url);
        }
    }
}