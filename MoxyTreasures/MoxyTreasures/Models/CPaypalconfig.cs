using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoxyTreasures.Models
{
    public static class CPaypalconfig
    {
        //Variables for storing ClientID and Client Secret Key
        public readonly static string ClientID;
        public readonly static string ClientSecret;

        //Constructor
        static CPaypalconfig()
        {
            var config = GetConfig();
            ClientID = config["ClientID"];
            ClientSecret = config["ClientSecret"];
        }

        // Getting Properties from from the web.config
        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }

        private static string GetAccessToken()
        {
            // Getting Access token from PayPal
            string accessToken = new OAuthTokenCredential(ClientID, ClientSecret, GetConfig()).GetAccessToken();
            return accessToken;
        }

        public static APIContext GetAPIContext()
        {
            // Return API context object by invoking it with acccess token
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }

    }
}