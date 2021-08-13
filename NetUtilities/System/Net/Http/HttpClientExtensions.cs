using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpClientExtensions
    {
        public static Task DownloadAsync(this HttpClient client, string uri, string path, int chunkSize = 1024, bool replaceFile = false, IProgress<double>? progress = null, CancellationToken token = default)
            => client.DownloadAsync(new Uri(uri), path, chunkSize, replaceFile, progress, token);

        public static async Task DownloadAsync(this HttpClient client, Uri uri, string path, int chunkSize = 1024, bool replaceFile = false, IProgress<double>? progress = null, CancellationToken token = default)
        {
            if (client is null)
                throw new ArgumentNullException(nameof(client));

            if (chunkSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(chunkSize), chunkSize, "The parameter cannot be negative or zero.");

            await new SynchronizationContextRemover();

            var response = await client.GetAsync(uri, token);
            response.EnsureSuccessStatusCode();

            if (replaceFile)
                File.Delete(path);

            using var memoryOwner = MemoryPool<byte>.Shared.Rent(chunkSize);
            using var fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.Read, chunkSize, true);

            var memory = memoryOwner.Memory[..chunkSize];
            var totalRead = 0;
            var stream = await response.Content.ReadAsStreamAsync(token);
            var totalLength = response.Content.Headers.ContentLength.GetValueOrDefault();

            while (await stream.ReadAsync(memory, token) is var bytesRead and > 0)
            {
                await fileStream.WriteAsync(memory, token);
                progress?.Report((totalRead += bytesRead) * 100.0 / totalLength);
            }
        }
    }
}
