using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Net.Security;

namespace HolidayHack2023_reportinator
{
    internal class Program
    {
        private static string hh_url = "https://hhc23-reportinator-dot-holidayhack2023.ue.r.appspot.com/check";

        private static void SetDefaultRequestHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("ContentType", "application/x-www-form-urlencoded");
            client.DefaultRequestHeaders.Add("Cookie", @"ReportinatorCookieYum =");
        }

        private static HttpClientHandler CreateHttpHandler()
        {
            return new HttpClientHandler
            {
                UseDefaultCredentials = true,
                AllowAutoRedirect = true,
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    switch (sslPolicyErrors)
                    {
                        case SslPolicyErrors.None:
                            return true;

                        default:
                            return false;
                    }
                }
            };
        }

        public static bool SendToPostMessageToReportinator(string inputData)
        {
            string responseErrorMessage = string.Empty;
            try
            {
                using (var httpClientHandler = CreateHttpHandler())
                using (var client = new HttpClient(httpClientHandler))
                {
                    SetDefaultRequestHeaders(client);

                    using (var data = new StringContent(inputData, Encoding.UTF8, "application/x-www-form-urlencoded"))
                    {
                        using (var responseMsg = client.PostAsync(hh_url, data).Result)
                        {
                            return responseMsg.IsSuccessStatusCode;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


        static void Main(string[] args)
        {
            for (int i = 0; i < 512; i++)
            {
                string bv = Convert.ToString(i, 2).PadLeft(9, '0');
                string testData = "input-1="  + bv.Substring(0, 1) + 
                                  "&input-2=" + bv.Substring(1, 1) +   
                                  "&input-3=" + bv.Substring(2, 1) +
                                  "&input-4=" + bv.Substring(3, 1) +
                                  "&input-5=" + bv.Substring(4, 1) +
                                  "&input-6=" + bv.Substring(5, 1) +
                                  "&input-7=" + bv.Substring(6, 1) +
                                  "&input-8=" + bv.Substring(7, 1) +
                                  "&input-9=" + bv.Substring(8, 1);

                Console.Write(testData);
                if (SendToPostMessageToReportinator(testData))
                {
                    Console.WriteLine(" FOUND, Hooray!!!!");
                    return;
                }
                else
                {
                    Console.WriteLine(" NOT FOUND");
                }
            }
        }
    }
}
