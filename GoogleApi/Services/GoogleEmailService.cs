using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using GoogleApi.Common;

namespace GoogleApi.Services
{
    public class GoogleEmailService
    {
        public static GmailService GetGmailService(string refreshToken)
        {
            GmailService gmailService = null;
            var userCredential = GoogleService.GetGoogleUserCredentialByRefreshToken(refreshToken);
            if (userCredential != null)
            {
                gmailService = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = userCredential,
                    ApplicationName = GoogleApiHelper.ApplicationName
                });
            }
            return gmailService;
        }
    }
}
