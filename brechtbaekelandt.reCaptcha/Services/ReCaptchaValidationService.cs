using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using brechtbaekelandt.reCaptcha.Services.Models;
using Newtonsoft.Json;

namespace brechtbaekelandt.reCaptcha.Services
{
    public class ReCaptchaValidationService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private readonly string _url = "";

        private readonly string _secretKey;

        public ReCaptchaValidationService(string secretKey)
        {
            this._secretKey = secretKey;
        }

        public async Task<ReCaptchaValidationResult> Validate(string reCaptchaResponse)
        {
            var content = new FormUrlEncodedContent(
                new[]
                {
                    new KeyValuePair<string, string>("secret", this._secretKey),
                    new KeyValuePair<string, string>("response", reCaptchaResponse)
                });

            var response = await this._httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);

            return response?.Content == null ? null : JsonConvert.DeserializeObject<ReCaptchaValidationResult>(await response.Content.ReadAsStringAsync());
        }
    }
}
