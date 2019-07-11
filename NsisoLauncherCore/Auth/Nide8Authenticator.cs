using System;
using System.Collections.Generic;
using System.Text;
using NsisoLauncherCore.Net.MojangApi.Api;
using NsisoLauncherCore.Net.MojangApi.Endpoints;
using NsisoLauncherCore.Net.MojangApi.Responses;
using static NsisoLauncherCore.Net.MojangApi.Responses.AuthenticateResponse;

namespace NsisoLauncherCore.Auth
{
    public class Nide8Authenticator : YggdrasilAuthenticator
    {
        public Nide8Authenticator(Credentials credentials) : base(credentials) { }
    }

    public class Nide8TokenAuthenticator : YggdrasilTokenAuthenticator
    {
        public Nide8TokenAuthenticator(string token, Uuid selectedProfileUUID, UserData userData) : base(token, selectedProfileUUID, userData) { }
        
    }
}
