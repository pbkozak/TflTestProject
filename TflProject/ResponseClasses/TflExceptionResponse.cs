using Newtonsoft.Json;
using System;

namespace TflRoadStatus.ResponseClasses
{

    public class TflExceptionResponse
    {
        [JsonProperty("$type")]
        public string type { get; set; }
        [JsonProperty("timestampUtc")]
        public DateTime timestampUtc { get; set; }
        [JsonProperty("exceptionType")]
        public string exceptionType { get; set; }
        [JsonProperty("httpStatusCode")]
        public int httpStatusCode { get; set; }
        [JsonProperty("httpStatus")]
        public string httpStatus { get; set; }
        [JsonProperty("relativeUri")]
        public string relativeUri { get; set; }
        [JsonProperty("message")]
        public string message { get; set; }
    }


}
