using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using F12Website.Models;

namespace F12Website.Utils
{
    public class ValidateReCaptcha
    {

        private const string VERIFY_URL = "https://www.google.com/recaptcha/api/siteverify";
        private const string VERIFY_URL_ENTERPRISE = "https://recaptchaenterprise.googleapis.com";

        public async Task<bool> IsCaptchaValid(string secretKey, string reCaptchaToken)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                    {
                        {"secret", secretKey},
                        {"response", reCaptchaToken},

                    };

                    var paramValues = new FormUrlEncodedContent(values);
                    var verifyReCaptcha = await client.PostAsync(VERIFY_URL, paramValues);
                    var reCaptchaResponseJson = await verifyReCaptcha.Content.ReadAsStringAsync();

                    var captchaResult = JsonSerializer.Deserialize<ReCaptchaResponseModel>(reCaptchaResponseJson);
                    return captchaResult.Success;


                }


            }
            catch (Exception ex)
            {
                return false;
            }



        }
    }
}
