using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;


namespace DeviceIdentity
{
    class Program
    {
        static RegistryManager registryManager;
        static string connectionString = "HostName=mclass21.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=32+rp32aGG0pyMd8v9zsrW0gU2C7Ic5BN9f0lh1HLjQ=";
       
        

        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            AddDeviceAsync().Wait();
            Console.ReadLine();

        }

        private static async Task AddDeviceAsync() {
            string deviceID = "mydeviceid1";
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceID));
            }
            catch (DeviceAlreadyExistsException ex)
            {
                device = await registryManager.GetDeviceAsync(deviceID);
            }
            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);

        }
    }
}
