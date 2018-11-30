namespace ImageStorage.Lambda.Warmup
{
    using System;
    using System.Net.Http;

    public class WebPDownloader : StandardDownloader
    {
        public WebPDownloader(Func<HttpClient> builder)
            : base(builder)
        {
        }

        protected override HttpRequestMessage Build(Uri url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            request.Headers.Accept.ParseAdd("image/webp");

            return request;
        }
    }
}