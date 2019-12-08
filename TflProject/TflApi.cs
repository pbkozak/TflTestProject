using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TflRoadStatus.ResponseClasses;

namespace TflProject
{
    public class TflApi
    {
        public string AppId;
        public string AppKey;

        public TflApi(string appId, string appKey)
        {
            // Set secure connection defaults
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            
            // Set data
            AppId = appId;
            AppKey = appKey;
        }

        public TflStatus CheckRoadStatus(string roadName)
        {
            const string roadStatusUrl = "https://api.tfl.gov.uk/Road/{0}/Status";
            var serializer = new JsonSerializer();
            var response = new TflStatus() { Error = true, Success = false };

            if(string.IsNullOrWhiteSpace(AppId) || string.IsNullOrWhiteSpace(AppKey))
            {
                response.Messages.Add("No credentials provided");
                return response;
            }

            // Build request
            var request = (HttpWebRequest)WebRequest.Create(string.Format(roadStatusUrl, roadName));
            request.ContentType = "application/json; charset=utf-8";
            request.Headers["app_key"] = AppKey;
            request.Headers["app_id"] = AppId;
            
            try
            {
                var httpResponse = request.GetResponse() as HttpWebResponse;
                using (var responseStream = httpResponse.GetResponseStream())
                {
                    var streamReader = new StreamReader(responseStream, Encoding.UTF8);
                    
                    // Parse JSON
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        var trr = serializer.Deserialize<List<TflRoadResponse>>(jsonReader);
                        response.Success = true;
                        response.Error = false;
                        foreach (var t in trr)
                        {
                            response.Messages.Add("Road status is " + t.statusSeverity);
                            response.Messages.Add("Road status description is " + t.statusSeverityDescription);
                        }   
                    }
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse webResponse = (HttpWebResponse)ex.Response;
                if (webResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    try
                    {
                        using (Stream responseStream = ex.Response.GetResponseStream())
                        {
                            StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
                            using (JsonReader jsonReader = new JsonTextReader(streamReader))
                            {
                                var tex = serializer.Deserialize<TflExceptionResponse>(jsonReader);
                                response.Error = false;
                                response.Messages.Add(tex.message);
                            }
                        }
                    }
                    catch (Exception ex2) { 
                        response.Messages.Add("Failed requesting road status: " + ex2.Message);
                    }
                }
                else
                {
                    response.Messages.Add("Failed requesting road status: " + ex.Message);
                }
            }

            return response;
        }
    }
}
