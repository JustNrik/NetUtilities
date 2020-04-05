using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    /// <summary>
    /// This class is a Singleton wrapper over <see cref="HttpClient"/>
    /// </summary>
    public static class GlobalHttpClient
    {
        private static readonly object _lock = new object();
        private static HttpClient? _client;
        private static volatile bool _initialized;

        private static HttpClient Client
        {
            get
            {
                EnsureInitialized();
                return _client!;
            }
        }

        /// <summary>
        /// Gets or sets the global Http proxy. This points directly to <see cref="HttpClient.DefaultProxy"/>
        /// </summary>
        public static IWebProxy WebProxy
        {
            get => HttpClient.DefaultProxy;
            set => HttpClient.DefaultProxy = value;
        }

        /// <summary>
        /// Gets or sets the default HTTP version used on subsequent requests made by this client.
        /// </summary>
        public static Version DefaultRequestVersion
        {
            get => Client.DefaultRequestVersion;
            set => Client.DefaultRequestVersion = value;
        }

        /// <summary>
        /// Gets the headers which should be sent with each request.
        /// </summary>
        public static HttpRequestHeaders DefaultRequestHeaders 
        {
            get => Client.DefaultRequestHeaders; 
        }

        /// <summary>
        /// Gets or sets the base address of Uniform Resource Identifier (URI) of the Internet resource used when sending requests.
        /// </summary>
        public static Uri BaseAddress 
        {
            get
            {
                lock (_lock)
                    return Client.BaseAddress;
            }
            set
            {
                lock (_lock)
                    Client.BaseAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of bytes to buffer when reading the response content.
        /// </summary>
        public static long MaxResponseContentBufferSize 
        { 
            get
            {
                lock (_lock)
                    return Client.MaxResponseContentBufferSize;
            }
            set
            {
                lock (_lock)
                    Client.MaxResponseContentBufferSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the timespan to wait before the request times out.
        /// </summary>
        public static TimeSpan Timeout
        { 
            get
            {
                lock (_lock)
                    return Client.Timeout;
            }
            set
            {
                lock (_lock)
                    Client.Timeout = value;
            }
        }

        /// <summary>
        /// Initializes the <see cref="HttpClient"/> calling the parameterless constructor.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when you call this method more than once.</exception>
        public static void Initialize()
        {
            lock (_lock)
            {
                if (_initialized)
                    throw new InvalidOperationException("You cannot initialize this client more than once.");

                _client = new HttpClient();
                _initialized = true;
            }
        }

        /// <summary>
        /// Initializes the <see cref="HttpClient"/> calling the constructor <see cref="HttpClient(HttpMessageHandler)"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the handler is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when you call this method more than once.</exception>
        /// <param name="messageHandler">The HTTP handler stack to use for sending requests.</param>
        public static void Initialize(HttpMessageHandler messageHandler)
        {
            lock (_lock)
            {
                if (_initialized)
                    throw new InvalidOperationException("You cannot initialize this client more than once.");

                _client = new HttpClient(messageHandler);
                _initialized = true;
            }
        }

        /// <summary>
        /// Cancel all pending requests on this client.
        /// </summary>
        public static void CancelPendingRequests()
            => Client.CancelPendingRequests();

        /// <summary>
        /// Send a DELETE request to the specified <see cref="Uri"/> as an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the request message was already sent by the client.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
            => Client.DeleteAsync(requestUri);

        /// <summary>
        /// Send a DELETE request to the specified <see cref="Uri"/> designated as a <see cref="string"/> as an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the request message was already sent by the client.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> DeleteAsync(string requestUri)
            => Client.DeleteAsync(requestUri);

        /// <summary>
        /// Send a DELETE request to the specified <see cref="Uri"/> with a <see cref="CancellationToken"/> as an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the request message was already sent by the client.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken)
            => Client.DeleteAsync(requestUri, cancellationToken);

        /// <summary>
        /// Send a DELETE request to the specified <see cref="Uri"/> designated as a <see cref="string"/> with a <see cref="CancellationToken"/> as an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the request message was already sent by the client.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken)
            => Client.DeleteAsync(requestUri, cancellationToken);

        /// <summary>
        /// Send a GET request to the specified <see cref="Uri"/> as an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> GetAsync(Uri requestUri)
            => Client.GetAsync(requestUri);

        /// <summary>
        /// Send a GET request to the specified <see cref="Uri"/> designated as a <see cref="string"/> as an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> GetAsync(string requestUri)
            => Client.GetAsync(requestUri);

        /// <summary>
        /// Send a GET request to the specified <see cref="Uri"/> with a <see cref="CancellationToken"/> as an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)
            => Client.GetAsync(requestUri, cancellationToken);

        /// <summary>
        /// Send a GET request to the specified <see cref="Uri"/> designated as a <see cref="string"/> with a <see cref="CancellationToken"/> as an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
            => Client.GetAsync(requestUri, cancellationToken);

        /// <summary>
        /// Send a GET request to the specified <see cref="Uri"/> with an HTTP completion option as an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="completionOption">An HTTP completion option value that indicates when the operation should be considered completed.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption)
            => Client.GetAsync(requestUri, completionOption);

        /// <summary>
        /// Send a GET request to the specified <see cref="Uri"/> designated as a <see cref="string"/> with an HTTP completion option as an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="completionOption">An HTTP completion option value that indicates when the operation should be considered completed.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption)
            => Client.GetAsync(requestUri, completionOption);

        /// <summary>
        /// Send a GET request to the specified <see cref="Uri"/> with an HTTP completion option and a cancellation token as an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="completionOption">An HTTP completion option value that indicates when the operation should be considered completed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
            => Client.GetAsync(requestUri, completionOption, cancellationToken);

        /// <summary>
        /// Send a GET request to the specified <see cref="Uri"/> designated as a <see cref="string"/> with an HTTP completion option and a cancellation token as an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="completionOption">An HTTP completion option value that indicates when the operation should be considered completed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
            => Client.GetAsync(requestUri, completionOption, cancellationToken);

        /// <summary>
        /// Send a GET request to the specified <see cref="Uri"/> and return the response body as a <see cref="byte"/>[] in an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<byte[]> GetByteArrayAsync(Uri requestUri)
            => Client.GetByteArrayAsync(requestUri);

        /// <summary>
        /// Send a GET request to the specified <see cref="Uri"/> designated as a <see cref="string"/> and return the response body as a <see cref="byte"/>[] in an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<byte[]> GetByteArrayAsync(string requestUri)
            => Client.GetByteArrayAsync(requestUri);

        /// <summary>
        /// Send a GET request to the specified <see cref="Uri"/> designated as a <see cref="string"/> and return the response body as a <see cref="Stream"/> in an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<Stream> GetStreamAsync(string requestUri)
            => Client.GetStreamAsync(requestUri);

        /// <summary>
        /// Send a GET request to the specified <see cref="Uri"/> and return the response body as a <see cref="Stream"/> in an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<Stream> GetStreamAsync(Uri requestUri)
            => Client.GetStreamAsync(requestUri);

        /// <summary>
        /// Send a GET request to the specified <see cref="Uri"/> and return the response body as a <see cref="string"/> in an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<string> GetStringAsync(Uri requestUri)
            => Client.GetStringAsync(requestUri);

        /// <summary>
        /// Send a GET request to the specified <see cref="Uri"/> designated as a <see cref="string"/> and return the response body as a <see cref="string"/> in an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the requestUri is <see langword="null"/>.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<string> GetStringAsync(string requestUri)
            => Client.GetStringAsync(requestUri);

        /// <summary>
        /// Sends a PATCH request with a <see cref="CancellationToken"/> as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content)
            => Client.PatchAsync(requestUri, content);

        /// <summary>
        /// Sends a PATCH request to a <see cref="Uri"/> designated as a <see cref="string"/> as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content)
            => Client.PatchAsync(requestUri, content);

        /// <summary>
        /// Sends a PATCH request with a <see cref="CancellationToken"/> as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
            => Client.PatchAsync(requestUri, content, cancellationToken);

        /// <summary>
        /// Sends a PATCH request with a <see cref="CancellationToken"/> to a <see cref="Uri"/> designated as a <see cref="string"/> as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
            => Client.PatchAsync(requestUri, content, cancellationToken);

        /// <summary>
        /// Send a POST request to the specified <see cref="Uri"/> as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="requestUri"/> is <see langword="null"/></exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
            => Client.PostAsync(requestUri, content);

        /// <summary>
        /// Sends a POST request to a <see cref="Uri"/> designated as a <see cref="string"/> as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="requestUri"/> is <see langword="null"/></exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
            => Client.PostAsync(requestUri, content);

        /// <summary>
        /// Sends a POST request with a <see cref="CancellationToken"/> to the specified <see cref="Uri"/> as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="requestUri"/> is <see langword="null"/></exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
            => Client.PostAsync(requestUri, content, cancellationToken);

        /// <summary>
        /// Sends a POST request with a <see cref="CancellationToken"/> to a <see cref="Uri"/> designated as a <see cref="string"/> as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="requestUri"/> is <see langword="null"/></exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
            => Client.PostAsync(requestUri, content, cancellationToken);

        /// <summary>
        /// Send a PUT request to the specified <see cref="Uri"/> as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="requestUri"/> is <see langword="null"/></exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
            => Client.PutAsync(requestUri, content);

        /// <summary>
        /// Send a PUT request to the specified <see cref="Uri"/> designated as a <see cref="string"/> as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="requestUri"/> is <see langword="null"/></exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
            => Client.PutAsync(requestUri, content);

        /// <summary>
        /// Send a PUT request to the specified <see cref="Uri"/> with a <see cref="CancellationToken"/> as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="requestUri"/> is <see langword="null"/></exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
            => Client.PutAsync(requestUri, content, cancellationToken);

        /// <summary>
        /// Send a PUT request to the specified <see cref="Uri"/> designated as a <see cref="string"/> with a <see cref="CancellationToken"/> as an asynchronous operation.
        /// </summary>
        /// <param name="requestUri">The Uri the request is sent to.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="requestUri"/> is <see langword="null"/></exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
            => Client.PutAsync(requestUri, content, cancellationToken);

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the request message was already sent by the client.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="request">The HTTP request message to send.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
            => Client.SendAsync(request);

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the request message was already sent by the client.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="request">The HTTP request message to send.</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Client.SendAsync(request, cancellationToken);

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the request message was already sent by the client.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="request">The HTTP request message to send.</param>
        /// <param name="completionOption">When the operation should complete (as soon as a response is available or after reading the whole response content).</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)
            => Client.SendAsync(request, completionOption);

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="request"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the request message was already sent by the client.</exception>
        /// <exception cref="HttpRequestException">Thrown when the request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <param name="request">The HTTP request message to send.</param>
        /// <param name="completionOption">When the operation should complete (as soon as a response is available or after reading the whole response content).</param>
        /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
            => Client.SendAsync(request, completionOption, cancellationToken);

        private static void EnsureInitialized()
        {
            if (!_initialized)
                throw new InvalidOperationException("You must call Initialize() or Initialize(HttpMessageHandler) before accessing this member.");
        }
    }
}
