using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using GoogleApi.Common;

namespace GoogleApi.Services
{
    public class GoogleService
    {
        public static ClientSecrets GoogleClientSecrets = new ClientSecrets()
        {
            ClientId = GoogleApiHelper.ClientId,
            ClientSecret = GoogleApiHelper.ClientSecret
        };

        public static IAuthorizationCodeFlow GoogleAuthorizationCodeFlow()
        {
            return new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = GoogleClientSecrets,
                Scopes = GoogleApiHelper.GetScopes()
            });
        }

        public static UserCredential GetGoogleUserCredentialByRefreshToken(string refreshToken)
        {
            UserCredential userCredential = null;
            IAuthorizationCodeFlow authorizationCodeFlow = GoogleAuthorizationCodeFlow();
            TokenResponse tokenResponse = new TokenResponse() { RefreshToken = refreshToken };
            if(authorizationCodeFlow != null && tokenResponse != null)
            {
                userCredential = new UserCredential(authorizationCodeFlow, "user", tokenResponse);
            }
            return userCredential;
        }
    }
}
