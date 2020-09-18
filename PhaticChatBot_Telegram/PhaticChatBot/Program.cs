using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Porter2Stemmer;

namespace PhaticChatBot
{
    class Program
    {
        private void oldMain()
        {
            //string input = Console.ReadLine();
            //EnglishPorter2Stemmer stemmer = new EnglishPorter2Stemmer();
            //StemmedWord word = stemmer.Stem(input);
            //string output = word.Value;
            //Console.WriteLine(output);

            CommandsHandler handler = new CommandsHandler("patterns.json");
            PatternsFinder finder = new PatternsFinder("patterns.json");
            while (handler.IsWorking)
            {
                string input = Console.ReadLine();
                if (input.StartsWith("/"))
                {
                    Console.WriteLine(handler.HandleCommands(input.Substring(1)));
                }
                else
                {
                    Console.WriteLine();
                }
            }
            //HashSet<string> verbs = new HashSet<string>();
            //using (StreamReader file = new StreamReader("verbs.txt"))
            //{
            //    while (!file.EndOfStream)
            //    {
            //        string[] buf = file.ReadLine().Split(' ');
            //        verbs.Add(buf[0]);
            //    }
            //}

            //using (StreamWriter file = new StreamWriter("patterns1.json"))
            //using (JsonTextWriter writer = new JsonTextWriter(file))
            //{
            //    writer.WriteStartArray();
            //    foreach (string v in verbs)
            //    {

            //        writer.WriteValue(v.Replace(":", ""));
            //    }
            //    writer.WriteEndArray();
            //}
        }
    }
}
