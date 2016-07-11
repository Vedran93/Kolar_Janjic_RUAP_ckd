using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CallRequestResponseService
{

    public class StringTable
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }

    
    class RequestResponse 
    {
        //Sadrži cijeli rezultat, iz njega treba izvuc klasu
        static string res;
        public static string Result
        {
            get
            {
                return res;
            }
            set
            {
                res = value;
            }
        }

        public static async Task InvokeRequestResponseService(string[] data)
        {
            //Podaci koje šaljemo servisu, samo kopiramo ovaj gore data u string matricu
            string[,] dataTosend = new string[1, data.Length];
            int i = 0;
            foreach (string temp in data)
            {
                dataTosend[0, i] = temp;
                i++;
            }

            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {
                    Inputs = new Dictionary<string, StringTable>() {
                        {
                            "input1",
                            new StringTable()
                            {
                                ColumnNames = new string[] {"age", "bp", "sg", "al", "su", "rbc", "pc", "pcc", "ba", "bgr", "bu", "sc", "sod", "pot", "hemo", "pcv", "wbcc", "rbcc", "htn", "dm", "cad", "appet", "pe", "ane", "class"},
                                Values = dataTosend     //dataToSend šaljemo serveru
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>()
                    {
                    }
                };
                const string apiKey = "/HaChWZk4R4S/B+2rVMm1B6Yq1VeYA6KvEAqVb390Oa7ukQYXFIhNZj6P+SDevm8joZ9oEdbMcznkoac6rawQg=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress = new Uri("	
https://ussouthcentral.services.azureml.net/workspaces/70dd861bdd004002a2489cc1289b1ca8/services/55de90c03c694183a286880ffffc72b8/execute?api-version=2.0&details=true");

                // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)


                HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Result = result;
                }
                else
                {
                    Result = (string.Format("The request failed with status code: {0}\n\n", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                    Result += (response.Headers.ToString() + "\n\n");

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Result += (responseContent + "\n\n");
                }
            }
        }
    }
}
