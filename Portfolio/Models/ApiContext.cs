using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Portfolio.Models
{
    public static class ApiContext
    {
        private const string Url = "https://api.github.com";
        private const string Header = "application/vnd.github.v3+json";
        private const string UserName = "TheBigTaco";

        public static async Task<List<Repository>> TopThreeRepos()
        {
            var client = new RestClient(Url);
            var request = new RestRequest("/search/repositories", Method.GET);
            request.AddHeader("Accept", Header);
            request.AddHeader("User-Agent", UserName);
            request.AddParameter("q", $"user:{UserName} fork:true");
            request.AddParameter("sort", "stars");
            request.AddParameter("order", "desc");

            var response = await GetResponseAsync(client, request) as RestResponse;
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            IQueryable<Repository> repoResults = JsonConvert.DeserializeObject<List<Repository>>(jsonResponse["items"].ToString()).AsQueryable();

            List<Repository> top3Repositories = repoResults.Take(3).ToList();
            return top3Repositories;
        }

        private static Task<IRestResponse> GetResponseAsync(RestClient client, RestRequest request)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }

    }
}
