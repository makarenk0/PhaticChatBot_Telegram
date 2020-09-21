using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Metrics;
using Microsoft.ApplicationInsights.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PhaticChatBot
{
    class CommandsHandler
    {
        private string _patternsFileNameJSON;
        private static string _helpInfo = "Use simple structures of sentences to interact with bot\nYou can ask or tell him something";
        private static string _keyForStorage = "1111";


        private const string URL =
        "https://api.applicationinsights.io/v1/apps/{0}/{1}/{2}?{3}";
        private string appid = "10243ab0-6f91-46ae-b4b4-1be081d17309";
        private string apikey = "yrvddfcqt8n2nnyx67euamqb7mdk4cwns2o5t0hz";

        public CommandsHandler(string patternsFileNameJSON)
        {
            _patternsFileNameJSON = patternsFileNameJSON;
            
        }
       
        public string HandleCommands(string command)
        {
            string[] words = command.Split(' ');
            switch (words[0])
            {
                case "start":
                    return "Start your conversation";
                case "help":
                    return _helpInfo;
                case "info":
                    return GetBotInfo();
                case "add" :
                    if (words.Length == 4) return words[1] == _keyForStorage ? AddNewWords(words) : "Invalid storage key!";
                    else return String.Concat("Please enter 4 parametrs: /add <storage key> <words type> <word to add>\n", GetBotInfo());
                case "metric":
                    if (words.Length == 1) return "Please enter 1 parametr: /metric <metric type>\nParametrs: cpu_percentage, cpu_average, memory_available," +
                             "requests_failed, requests_failed, requests_queue, requests_count";
                    switch (words[1])
                    {
                        case "cpu_percentage":
                            return readTelemetryResponse("performanceCounters/processCpuPercentage", "avg") + " %";
                        case "cpu_average":
                            return readTelemetryResponse("customEvents/custom/AverageCPUUsage", "avg") + " %";
                        case "memory_available":
                            return readTelemetryResponse("performanceCounters/memoryAvailableBytes", "avg") + " bytes";
                        case "requests_failed":
                            return readTelemetryResponse("requests/failed", "sum");
                        case "requests_queue":
                            return readTelemetryResponse("performanceCounters/requestsInQueue", "avg");
                        case "requests_count":
                            return readTelemetryResponse("requests/count", "sum");
                    }
                    return "No such metric!";
                default:
                    return "Command not found!";
            }
        }

        private string AddNewWords(string[] words)
        {
            using (StreamWriter file = new StreamWriter(_patternsFileNameJSON))
            using (JsonTextWriter reader = new JsonTextWriter(file))
            {
                
            }
            return "Is developing now";
        }

        private string GetBotInfo()
        {
            Dictionary<string, int> counts = new Dictionary<string, int>();
            string output = "I know:\n";

            using (StreamReader file = File.OpenText(_patternsFileNameJSON))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                reader.Read();
                string propertyName = "";
                while (reader.Read())
                {
                    string tokenType = reader.TokenType.ToString();
                    if (tokenType == "PropertyName")
                    {
                        propertyName = reader.Value.ToString();
                    }
                    else if (tokenType == "StartObject") //starting new pattern type reading
                    {
                        counts.Add(propertyName, 0);
                        while (true)
                        {
                            reader.Read();
                            tokenType = reader.TokenType.ToString();
                            if (tokenType == "EndObject") break;
                            reader.Read();
                            ++counts[propertyName];
                        }
                    }
                    else if (tokenType == "StartArray") //starting new words type reading
                    {
                        counts.Add(propertyName, 0);
                        while (true)
                        {
                            reader.Read();
                            tokenType = reader.TokenType.ToString();
                            if (tokenType == "EndArray") break;
                            ++counts[propertyName];
                        }
                    }
                }
            }
            foreach(KeyValuePair<string, int> keyValue in counts)
            {
                output = String.Concat(output, keyValue.Value, " ", keyValue.Key, " ,\n");
            }
            return output;
        }

        private string readTelemetryResponse(string telemetryType, string property)
        {
            var obj = JObject.Parse(GetTelemetry(appid, apikey, "metrics", telemetryType, ""));
            return obj["value"][telemetryType][property].ToString();
        }

        public static string GetTelemetry(string appid, string apikey,
                string queryType, string queryPath, string parameterString)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-api-key", apikey);
            var req = string.Format(URL, appid, queryType, queryPath, parameterString);
            HttpResponseMessage response = client.GetAsync(req).Result;
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                return response.ReasonPhrase;
            }
        }

    }
}
