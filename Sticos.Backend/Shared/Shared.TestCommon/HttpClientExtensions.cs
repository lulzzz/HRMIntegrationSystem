using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared.Services.Extensions;

namespace Shared.TestCommon
{
    public static class HttpClientExtensions
    {
        public static async Task<TOut> GetAsyncAndDeserialize<TOut>(this HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Response from GET '{url}':");
            Debug.WriteLine(responseJson);
            Debug.WriteLine("");
            var deserializedObject = JsonConvert.DeserializeObject<TOut>(responseJson);
            return deserializedObject;
        }

        public static async Task<TOut> PostAsyncAndDeserialize<TOut, TIn>(this HttpClient client, string url, TIn model)
        {
            var response = await client.PostAsJsonAsync(url, model);
            response.EnsureSuccessStatusCode();
            var responseJson = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"Response from POST '{url}':");
            Debug.WriteLine(responseJson);
            Debug.WriteLine("");
            var deserializedObject = JsonConvert.DeserializeObject<TOut>(responseJson);
            return deserializedObject;
        }
    }
}
