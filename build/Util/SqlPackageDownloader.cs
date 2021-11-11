using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Build.Util
{
    internal class SqlPackageDownloader
    {
        private static readonly SocketsHttpHandler HttpClientHandler = new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(1)
        };

        public async Task<string> GetPage(string url)
        {
            using var httpClient = new HttpClient(HttpClientHandler, disposeHandler: false) ;
            using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseContentRead);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
