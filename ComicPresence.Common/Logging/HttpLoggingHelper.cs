using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ComicPresence.Common.Logging
{
    public static class HttpLoggingHelper
    {
        /// <summary>
        /// Turns an HTTP request and response into a string containing both
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requestBody">Can be null</param>
        /// <param name="response"></param>
        /// <param name="responseBody">Can be null</param>
        /// <returns></returns>
        public static string StringFromHttpRequestAndResponse(HttpWebRequest request, string requestBody, HttpWebResponse response, string responseBody)
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.Append(StringFromHttpRequest(request, requestBody));

            messageBuilder.Append("\r\n");
            messageBuilder.Append("\r\n");
            messageBuilder.Append("\r\n");

            if (response != null)
            {
                messageBuilder.Append(StringFromHttpResponse(response, responseBody));
            }
            else
            {
                messageBuilder.Append("Response object: null");
            }

            return messageBuilder.ToString();
        }

        public static string StringFromHttpRequest(HttpWebRequest request, string requestBody)
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.Append(request.Method);
            messageBuilder.Append(" ");
            messageBuilder.Append(request.RequestUri.ToString());
            messageBuilder.Append(" HTTP/");
            messageBuilder.Append(request.ProtocolVersion);
            messageBuilder.Append("\r\n");

            foreach (string headerName in request.Headers.AllKeys)
            {
                messageBuilder.Append(headerName);
                messageBuilder.Append(": ");
                messageBuilder.Append(request.Headers[headerName]);
                messageBuilder.Append("\r\n");
            }

            messageBuilder.Append("\r\n");

            messageBuilder.Append(requestBody);

            return messageBuilder.ToString();
        }

        public static string StringFromHttpRequestMessage(HttpRequestMessage request, string requestBody)
        {
            StringBuilder messageBuilder = new StringBuilder();
            messageBuilder.Append(request.Method);
            messageBuilder.Append(" ");
            messageBuilder.Append(request.RequestUri.ToString());
            messageBuilder.Append(" HTTP/");
            messageBuilder.Append(request.Version.ToString());
            messageBuilder.Append("\r\n");

            foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
            {
                foreach (string value in header.Value)
                {
                    messageBuilder.Append(header.Key);
                    messageBuilder.Append(": ");
                    messageBuilder.Append(value);
                    messageBuilder.Append("\r\n");
                }
            }

            messageBuilder.Append("\r\n");

            messageBuilder.Append(requestBody);

            return messageBuilder.ToString();
        }

        public static string StringFromHttpResponse(HttpWebResponse response, string responseBody)
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.Append("HTTP/");
            messageBuilder.Append(response.ProtocolVersion);
            messageBuilder.Append(" ");
            messageBuilder.Append((int)response.StatusCode);
            messageBuilder.Append(" ");
            messageBuilder.Append(response.StatusDescription);
            messageBuilder.Append("\r\n");

            foreach (string headerName in response.Headers.AllKeys)
            {
                messageBuilder.Append(headerName);
                messageBuilder.Append(": ");
                messageBuilder.Append(response.Headers[headerName]);
                messageBuilder.Append("\r\n");
            }

            messageBuilder.Append("\r\n");

            messageBuilder.Append(responseBody);

            return messageBuilder.ToString();
        }

        public static string StringFromHttpResponseMessage(HttpResponseMessage response, string responseBody)
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.Append("HTTP/");
            messageBuilder.Append(response.Version.ToString());
            messageBuilder.Append(" ");
            messageBuilder.Append((int)response.StatusCode);
            messageBuilder.Append(" ");
            messageBuilder.Append(response.ReasonPhrase);
            messageBuilder.Append("\r\n");

            foreach (KeyValuePair<string, IEnumerable<string>> header in response.Headers)
            {
                foreach (string value in header.Value)
                {
                    messageBuilder.Append(header.Key);
                    messageBuilder.Append(": ");
                    messageBuilder.Append(value);
                    messageBuilder.Append("\r\n");
                }
            }

            messageBuilder.Append("\r\n");

            messageBuilder.Append(responseBody);

            return messageBuilder.ToString();
        }

    }
}
