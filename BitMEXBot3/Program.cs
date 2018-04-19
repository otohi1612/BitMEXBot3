using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using BitMEX;

namespace BitMEXBot3
{
    class Program
    {
#if DEBUG
        private static string DefaultConfigFile = "../../../config.json";
#else
        private static string DefaultConfigFile = "./config.json";
#endif

        public class Config
        {
            public string apiKey1 = "";
            public string apiSecret1 = "";
            public string apiKey2 = "";
            public string apiSecret2 = "";
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Started.");

            Config config = LoadConfig(DefaultConfigFile);
            if (config == null)
            {
                Console.WriteLine("Cannot read config file.");
                WaitAndGo("Press Enter to exit...");
            }

            var client = new BitMEXApi(config.apiKey1, config.apiSecret1);
            Console.WriteLine(client.GetOrders());

            var quote = client.GetQuote();
            Console.WriteLine("Current Quote is BidPrice:{0}, AskPrice:{1}", quote.bidPrice, quote.askPrice);
            
            
            WaitAndGo("Press Enter to exit...");
        }

        static void WaitAndGo(string message = null)
        {
            Console.WriteLine(message ?? "Press Enter to continue...");
            Console.ReadLine();
        }



        static Config LoadConfig(string configFilePath)
        {
            Config config;

            try
            {
                using (var configFileStream = new FileStream(path: configFilePath, mode: FileMode.Open))
                using (var configReadStream = new StreamReader(configFileStream))
                {
                    config = JsonConvert.DeserializeObject<Config>(configReadStream.ReadToEnd());
                }
            }
            catch
            {
                return null;
            }

            return config;
        }
    }
}
