using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using GoogleApi.Common;
using GoogleApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoogleApi.Services
{
    public class GoogleEmailService
    {
        public static string NextPageToken = null;

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

        public static async Task<List<GoogleEmail>> GetEmailService(string refreshToken)
        {
            List<GoogleEmail> googleEmails = new List<GoogleEmail>();

            var gmailService = GetGmailService(refreshToken);

            do
            {
                var emailListRequest = gmailService.Users.Messages.List("me");
                emailListRequest.LabelIds = "INBOX";
                emailListRequest.Q = "category:primary";
                emailListRequest.IncludeSpamTrash = false;
                if (NextPageToken != null) emailListRequest.PageToken = NextPageToken;
                var emailListResponse = await emailListRequest.ExecuteAsync();

                if (emailListResponse != null && emailListResponse.Messages != null)
                {
                    foreach (var email in emailListResponse.Messages)
                    {
                        GoogleEmail googleEmail = new GoogleEmail();
                        googleEmail.MessageId = email.Id;

                        var emailInfoRequest = gmailService.Users.Messages.Get("me", email.Id);
                        var emailInfoResponse = await emailInfoRequest.ExecuteAsync();
                        if (emailInfoResponse != null)
                        {
                            if (emailInfoResponse.InternalDate.HasValue)
                            {
                                googleEmail.Date = DateTimeOffset.FromUnixTimeMilliseconds(emailInfoResponse.InternalDate.Value).DateTime;
                            }

                            foreach (var mailParts in emailInfoResponse.Payload.Headers)
                            {
                                if (mailParts.Name == "Subject")
                                {
                                    googleEmail.Subject = mailParts.Value;
                                }
                                else if (mailParts.Name == "From")
                                {
                                    googleEmail.From = mailParts.Value;
                                }
                                //else if (mailParts.Name == "Date")
                                //{
                                //    googleEmail.Date = DateTime.Parse(mailParts.Value);
                                //}
                                if (googleEmail.Date != null && !string.IsNullOrEmpty(googleEmail.From) && emailInfoResponse.Payload.Parts != null)
                                {
                                    foreach (MessagePart messagePart in emailInfoResponse.Payload.Parts)
                                    {
                                        if (messagePart.MimeType == "text/html")
                                        {
                                            byte[] data = GoogleApiHelper.FromBase64UrlDecode(messagePart.Body.Data);
                                            string decodeString = Encoding.UTF8.GetString(data);
                                            googleEmail.Body = decodeString;
                                        }
                                    }
                                }
                            }
                        }
                        googleEmails.Add(googleEmail);
                    }
                    NextPageToken = emailListResponse.NextPageToken;
                }
            } while (NextPageToken != null);

            return googleEmails;
        }
    }
}
