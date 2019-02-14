using System;
using System.Collections.Generic;
using System.Text;
using NsisoLauncherCore.Net.MojangApi.Api;
using NsisoLauncherCore.Net.MojangApi.Endpoints;
using NsisoLauncherCore.Net.MojangApi.Responses;

namespace NsisoLauncherCore.Auth
{
    public class Nide8Authenticator : YggdrasilAuthenticator
    {
        public Nide8Authenticator(Credentials credentials) : base(credentials) { }
    }

    public class Nide8TokenAuthenticator : YggdrasilTokenAuthenticator
    {
        public Nide8TokenAuthenticator(string token, Uuid uuid, AuthenticateResponse.UserData userdata) : base(token, uuid, userdata) { }
        
    }
}
