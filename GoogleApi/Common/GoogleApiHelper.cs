using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GoogleApi.Common
{
    public class GoogleApiHelper
    {
        public static string ApplicationName = "Google API DotNetCore Web Client";
        public static string ClientId = "455974923277-06m7cfl3pcfvhfl3ma1lkfgct7lnb54c.apps.googleusercontent.com";
        public static string ClientSecret = "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";
        public static string RedirectUri = "https://localhost:44334/Home/OauthCallback";
        public static string OauthUri = "https://accounts.google.com/o/oauth2/auth?";
        public static string TokenUri = "https://accounts.google.com/o/oauth2/token";

        public static List<string> GetScopes()
        {
            List<string> scopes = new List<string>();
            scopes.Add("https://www.googleapis.com/auth/userinfo.email");
            scopes.Add("https://www.googleapis.com/auth/userinfo.profile");
            scopes.Add("https://www.googleapis.com/auth/gmail.send");
            scopes.Add("https://www.googleapis.com/auth/gmail.modify");
            scopes.Add("https://www.googleapis.com/auth/gmail.compose");
            scopes.Add("https://mail.google.com/");
            scopes.Add("https://www.googleapis.com/auth/gmail.addons.current.action.compose");
            return scopes;
        }

        public static string GetAuthScopes()
        {
            string scopes = string.Empty;
            foreach (var scope in GetScopes())
            {
                scopes += scope + " ";
            }
            return scopes;
        }

        public static string GetOauthUri(string extraParam)
        {
            StringBuilder sbUri = new StringBuilder(OauthUri);
            sbUri.Append("client_id=" + ClientId);
            sbUri.Append("&redirect_uri=" + RedirectUri);
            sbUri.Append("&response_type=" + "code");
            sbUri.Append("&scope=" + GetAuthScopes());
            sbUri.Append("&access_type=" + "offline");
            sbUri.Append("&state=" + extraParam);
            sbUri.Append("&approval_prompt=" + "force");

            return sbUri.ToString();
        }

        public static async Task<GoogleToken> GetTokenByCode(string code)
        {
            GoogleToken token = null;

            var postData = new
            {
                code = code,
                client_id = ClientId,
                client_secret = ClientSecret,
                redirect_uri = RedirectUri,
                grant_type = "authorization_code"
            };

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(TokenUri, content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        token = JsonConvert.DeserializeObject<GoogleToken>(responseString);
                    }
                }
            }

            return token;
        }
    }
}
