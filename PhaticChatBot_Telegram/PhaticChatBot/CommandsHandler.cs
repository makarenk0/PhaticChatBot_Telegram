using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PhaticChatBot
{
    class CommandsHandler
    {
        private bool _working;
        private string _patternsFileNameJSON;

        #region Getters&Setters
        public bool IsWorking
        {
            get { return _working; }
            set { _working = value; }
        }

        #endregion

        public CommandsHandler(string patternsFileNameJSON)
        {
            IsWorking = true;
            _patternsFileNameJSON = patternsFileNameJSON;
        }
       
        public string HandleCommands(string command)
        {
            switch (command)
            {
                case "quit":
                    IsWorking = false;
                    break;
                case "info":
                    return GetBotInfo();
                default:
                    return "Command not found!";
            }
            return "Success!";
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
    }
}
