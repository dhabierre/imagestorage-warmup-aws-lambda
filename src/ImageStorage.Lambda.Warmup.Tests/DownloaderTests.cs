namespace ImageStorage.Lambda.Warmup.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Net.Http;
    using Xunit;

    public class DownloaderTests : IDisposable
    {
        private const string ImageUrl = "https://www.google.fr/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png";

        private readonly IList<string> imageTempPaths = new List<string>();

        [Fact]
        public void StandardDownloader()
        {
            using (var downloader = new WebPDownloader(() => new HttpClient()))
            {
                var stream = downloader.DownloadAsync(new Uri(ImageUrl)).Result;
                var imageTempName = $"{Path.GetTempFileName()}.jpg";

                imageTempPaths.Add(imageTempName);

                using (var image = Image.FromStream(stream))
                {
                    image.Save(imageTempName);
                }

                Assert.True(File.Exists(imageTempName));
            }
        }

        [Fact]
        public void WebPDownloader()
        {
            using (var downloader = new WebPDownloader(() => new HttpClient()))
            {
                var stream = downloader.DownloadAsync(new Uri(ImageUrl)).Result;
                var imageTempName = $"{Path.GetTempFileName()}.jpg";

                imageTempPaths.Add(imageTempName);

                using (var image = Image.FromStream(stream))
                {
                    image.Save(imageTempName);
                }

                Assert.True(File.Exists(imageTempName));
            }
        }

        public void Dispose()
        {
            foreach (var imageTempPath in this.imageTempPaths)
            {
                try
                {
                    File.Delete(imageTempPath);
                }
                catch
                {
                    // nothing to do
                }
            }
        }
    }
}
