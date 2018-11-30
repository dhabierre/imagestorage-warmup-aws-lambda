[assembly: Amazon.Lambda.Core.LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ImageStorage.Lambda.Warmup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Amazon.Lambda.Core;
    using Amazon.Lambda.SQSEvents;

    public class Function : IDisposable
    {
        private readonly IEnumerable<IDownloader> downloaders;

        public Function()
        {
            this.downloaders = new[]
            {
                new StandardDownloader(() => new HttpClient()),
                new WebPDownloader(() => new HttpClient())
            };
        }

        public async Task FunctionHandler(SQSEvent @event, ILambdaContext context)
        {
            foreach (var message in @event.Records)
            {
                await ProcessMessageAsync(message, context);
            }
        }

        private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
        {
            var url = message.Body ?? string.Empty;

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                context.Logger.LogLine($"Url '{url}' is not a well formed uri, exiting!");

                return;
            }

            context.Logger.LogLine($"Processing '{url}'...");

            var uri = new Uri(url);

            var tasks = new List<Task>(this.downloaders.Count());

            foreach (var downloader in this.downloaders)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    try
                    {
                        using (var stream = downloader.DownloadAsync(uri).Result)
                        {
                            stream.Close();
                        }
                    }
                    catch (Exception x)
                    {
                        context.Logger.LogLine($"Exception while processing '{uri}' with {downloader.GetType().FullName}: {x.Message}");
                    }
                }));
            }

            await Task.WhenAll(tasks.ToArray());
        }

        public void Dispose()
        {
            foreach (var downloader in this.downloaders)
            {
                downloader.Dispose();
            }
        }
    }
}
