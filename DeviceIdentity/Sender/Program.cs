using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;


namespace Sender
{
    class Program
    {

        static DeviceClient deviceClient;
        static string connectionString;
        static string deviceID;


        static void Main(string[] args)
        {
            deviceClient = DeviceClient.Create("mclass21.azure-devices.net", new DeviceAuthenticationWithRegistrySymmetricKey("mydeviceid1", "Ykrk8+3L6cjLJ8fdK/fhzXWrSzb6lZFJ/rgguXmzquk="));
            deviceClient.ProductInfo = "humidityapp";
            SendMessages();

            Console.ReadLine();
        }


        private static async void SendMessages()
        {
            int mid = 1;
            Random rnd = new Random();           

            while (true) {
                
                double output = rnd.NextDouble() * 10;
                double h = output * 3;

                var tempdata = new {
                    deviceName = "mydeviceid1",
                    tempdata = output,
                    humidity = h,
                    messageID = mid++

                };
                var messageStr = JsonConvert.SerializeObject(tempdata);

                var message = new Message(Encoding.ASCII.GetBytes(messageStr));

                message.Properties.Add("humidityAlert", (h > 20) ? "true" : "false");
                await deviceClient.SendEventAsync(message);
                Console.WriteLine(messageStr);

                await Task.Delay(1000);







            }
        }
    }
}
