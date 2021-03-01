using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;

namespace csharp_demo
{
  public static class RequestUtils
  {
    private static readonly HttpClient client = new HttpClient();

    public static R PostForEntity<R>(string uri, IDictionary<string, string> paramDict)
    {
      var content = new FormUrlEncodedContent(paramDict);
      var response = client.PostAsync(uri, content).Result;
      var responseString = response.Content.ReadAsStringAsync().Result;

      return JsonConvert.DeserializeObject<R>(responseString);
    }
  }
}