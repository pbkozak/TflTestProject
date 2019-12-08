using System;
using System.Configuration;

namespace TflProject
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Set up
                var tflApiKey = ConfigurationManager.AppSettings["tflAppKey"];
                var tflApiId = ConfigurationManager.AppSettings["tflAppId"];
                var tflApi = new TflApi(tflApiId, tflApiKey);

                // Check status of A2 road
                Console.WriteLine("The status of the A2 is as follows:");
                var knownRoadResponse = tflApi.CheckRoadStatus("A2");
                foreach (var s in knownRoadResponse.Messages)
                    Console.WriteLine(s);
                Console.WriteLine("");

                // Check status of A233 (non existing road) road
                Console.WriteLine("The status of the A233 is as follows");
                var unknownRoadResponse = tflApi.CheckRoadStatus("A233");
                foreach (var s in unknownRoadResponse.Messages)
                    Console.WriteLine(s);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            Console.WriteLine("All finished, please press any key to exit..");
            Console.ReadKey();

        }
    }
}
