using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpClientExtensions
    {
        public static Task DownloadAsync(this HttpClient client, string uri, string path, int chunkSize = 1024, IProgress<double>? progress = null, CancellationToken token = default)
            => client.DownloadAsync(new Uri(uri), path, chunkSize, progress, token);

        public static async Task DownloadAsync(this HttpClient client, Uri uri, string path, int chunkSize = 1024, IProgress<double>? progress = null, CancellationToken token = default)
        {
            if (client is null)
                throw new ArgumentNullException(nameof(client));

            if (chunkSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(chunkSize), chunkSize, "The parameter cannot be negative or zero.");

            var response = await client.GetAsync(uri, token);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException("Something went wrong", null, response.StatusCode);

            using var memoryOwner = MemoryPool<byte>.Shared.Rent(chunkSize);
            using var fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.Read, chunkSize, true);

            var memory = memoryOwner.Memory.Slice(0, chunkSize);
            var totalRead = 0;
            var stream = await response.Content.ReadAsStreamAsync(token);
            var totalLength = (double)response.Content.Headers.ContentLength.GetValueOrDefault();

            while (await stream.ReadAsync(memory, token) is var bytesRead and > 0)
            {
                await fileStream.WriteAsync(memory, token);
                progress?.Report((totalRead += bytesRead) * 100 / totalLength);
            }
        }
    }
}
