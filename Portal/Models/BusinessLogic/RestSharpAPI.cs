using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestSharp;
using RestSharp.Deserializers;
using System.Net;

namespace Portal.Models.BusinessLogic
{
    public static class RestSharpAPI
    {
        private static RestClient openapiclient = new RestClient("https://hopprlab.com/openapi");
        private static RestRequest request;
        private static JsonDeserializer deserializer = new JsonDeserializer();

        public static HttpStatusCode checkDumpingStatus(this string user_id)
        {
            request = new RestRequest("/checkDumpingStatus/" + user_id , Method.GET);
            request.RequestFormat = DataFormat.Json;

            var response = openapiclient.Execute(request);
            var statusCode = response.StatusCode;

            return statusCode;
        }
    }
}