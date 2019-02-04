using System;
using System.Threading.Tasks;
using NsisoLauncherCore.Net.MojangApi.Api;
using static NsisoLauncherCore.Net.MojangApi.Responses.AuthenticateResponse;

namespace NsisoLauncherCore.Auth
{
    public class OfflineAuthenticator : IAuthenticator
    {
        public string Displayname { get; set; }

        public AuthenticateResult DoAuthenticate()
        {
            Uuid uuid = new Uuid()
            {
                PlayerName = Displayname,
                Value = Guid.NewGuid().ToString("N")
            };

            string accessToken = Guid.NewGuid().ToString("N");

            UserData userdata = new UserData()
            {
                Uuid = Guid.NewGuid().ToString("N"),
            };

            return new AuthenticateResult(AuthState.SUCCESS) { AccessToken = accessToken, UUID = uuid, UserData = userdata };
        }

        public async Task<AuthenticateResult> DoAuthenticateAsync()
        {
            return await Task.Factory.StartNew(() => { return DoAuthenticate(); });
        }

        public OfflineAuthenticator(string displayname)
        {
            this.Displayname = displayname;
        }
    }
}
