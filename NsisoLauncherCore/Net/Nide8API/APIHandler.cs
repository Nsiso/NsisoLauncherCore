using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NsisoLauncherCore.Net.Nide8API
{
    public class APIHandler
    {
        public string BaseURL { get; private set; }
        public string ServerID { get; private set; }
        public APIHandler(string id)
        {
            ServerID = id;
            BaseURL = string.Format("https://auth2.nide8.com:233/{0}/", ServerID);
        }

        public async Task<APIModules> GetInfoAsync()
        {
            string json = await APIRequester.HttpGetStringAsync(BaseURL);
            return await Task.Factory.StartNew(() =>
            {
                return JsonConvert.DeserializeObject<APIModules>(json);
            });
        }

        public Task UpdateBaseURL()
        {
            return Task.Factory.StartNew(async () =>
            {
                APIModules module = JsonConvert.DeserializeObject<APIModules>(await APIRequester.HttpGetStringAsync(BaseURL));
                if (!string.IsNullOrWhiteSpace(module.APIRoot))
                {
                    this.BaseURL = module.APIRoot;
                }
            });  
        }
    }
}
