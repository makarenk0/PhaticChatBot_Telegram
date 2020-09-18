using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace PhaticChatBot
{
    class CommandsHandler
    {
        private string _patternsFileNameJSON;
        private static string _helpInfo = "nothing here yet";
        private static string _keyForStorage = "14032001";

       


        public CommandsHandler(string patternsFileNameJSON)
        {
            _patternsFileNameJSON = patternsFileNameJSON;
        }
       
        public string HandleCommands(string command)
        {
            string[] words = command.Split(' ');
            switch (words[0])
            {
                case "help":
                    return _helpInfo;
                case "info":
                    return GetBotInfo();
                case "add" :
                    if (words.Length == 4) return words[1] == _keyForStorage ? AddNewWords(words) : "Invalid storage key!";
                    else return String.Concat("Please enter 4 parametrs: /add <storage key> <words type> <word to add>\n", GetBotInfo());
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
    }
}
