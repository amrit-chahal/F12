using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace F12Website.Models
{
    public class ReCaptchaResponseModel
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("error-codes")]
        public IEnumerable<string> ErrorCodes { get; set; }

        [JsonPropertyName("challenge_ts")]
        public DateTime ChallengeTime { get; set; }
        [JsonPropertyName("hostname")]
        public string HostName { get; set; }

    }
}
