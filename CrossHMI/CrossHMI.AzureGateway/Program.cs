// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Provisioning.Client;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;

namespace CrossHMI.AzureGateway
{
    class Program
    {
        // The Provisioning Hub IDScope.

        // For this sample either:
        // - pass this value as a command-prompt argument
        // - set the DPS_IDSCOPE environment variable 
        // - create a launchSettings.json (see launchSettings.json.template) containing the variable
        private static string _sIdScope = "0ne000BE340";
        //private static string _sIdScope = "0ne000BE340";

        // In your Device Provisioning Service please go to "Manage enrollments" and select "Individual Enrollments".
        // Select "Add individual enrollment" then fill in the following:
        // Mechanism: Symmetric Key
        // Auto-generate keys should be checked
        // DeviceID: iothubSymmetricKeydevice1

        // Symmetric Keys may also be used for enrollment groups.
        // In your Device Provisioning Service please go to "Manage enrollments" and select "Enrollment Groups".
        // Select "Add enrollment group" then fill in the following:
        // Group name: <your  group name>
        // Attestation Type: Symmetric Key
        // Auto-generate keys should be checked
        // You may also change other enrollment group parameters according to your needs

        private const string GlobalDeviceEndpoint = "global.azure-devices-provisioning.net";

        //These are the two keys that belong to your enrollment group. 
        // Leave them blank if you want to try this sample for an individual enrollment instead
        private const string EnrollmentGroupPrimaryKey = "";
        private const string EnrollmentGroupSecondaryKey = "";

        //registration id for enrollment groups can be chosen arbitrarily and do not require any portal setup. 
        //The chosen value will become the provisioned device's device id.
        //
        //registration id for individual enrollments must be retrieved from the portal and will be unrelated to the provioned
        //device's device id
        //
        //This field is mandatory to provide for this sample
        //private static string _registrationId = "1le4nmmt96n";
        private static string _registrationId = "11fbl9qjofg";

        //These are the two keys that belong to your individual enrollment. 
        // Leave them blank if you want to try this sample for an individual enrollment instead
        //private const string IndividualEnrollmentPrimaryKey = "+2rpjJpbElx85bK6CT6GOEDdLouZbx295y3JK5t7FPg=";
        //private const string IndividualEnrollmentSecondaryKey = "OeSA9b2EKoUl4zONFVQd/bS+sNJxoR6auTq4gII1gJs=";
        private const string IndividualEnrollmentPrimaryKey = "sU4lwVTiSWu4gbWpawiIhSKcfPso8D7v7MrTYdsGu/c=";
        private const string IndividualEnrollmentSecondaryKey = "LSUuYdh3yAcTnC3pqmEJOJuInqou6AWlWn72oRXkzv4=";

        public static async Task Main(string[] args)
        {
            var primaryKey = IndividualEnrollmentPrimaryKey;
            var secondaryKey = IndividualEnrollmentSecondaryKey;


            using (var security = new SecurityProviderSymmetricKey(_registrationId, primaryKey, secondaryKey))

                // Select one of the available transports:
                // To optimize for size, reference only the protocols used by your application.
            using (var transport = new ProvisioningTransportHandlerAmqp(TransportFallbackType.TcpOnly))
                // using (var transport = new ProvisioningTransportHandlerHttp())
                // using (var transport = new ProvisioningTransportHandlerMqtt(TransportFallbackType.TcpOnly))
                // using (var transport = new ProvisioningTransportHandlerMqtt(TransportFallbackType.WebSocketOnly))
            {
                ProvisioningDeviceClient provClient =
                    ProvisioningDeviceClient.Create(GlobalDeviceEndpoint, _sIdScope, security, transport);

                await RunSampleAsync(provClient, security);
            }

            Console.WriteLine("Enter any key to exit");
            Console.ReadLine();
        }

        /// <summary>
        /// Generate the derived symmetric key for the provisioned device from the enrollment group symmetric key used in attestation
        /// </summary>
        /// <param name="masterKey">Symmetric key enrollment group primary/secondary key value</param>
        /// <param name="registrationId">the registration id to create</param>
        /// <returns>the primary/secondary key for the member of the enrollment group</returns>
        public static string ComputeDerivedSymmetricKey(byte[] masterKey, string registrationId)
        {
            using (var hmac = new HMACSHA256(masterKey))
            {
                return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(registrationId)));
            }
        }

        public static async Task RunSampleAsync(ProvisioningDeviceClient provisioningDeviceClient,
            SecurityProvider security)
        {
            Console.WriteLine($"RegistrationID = {security.GetRegistrationID()}");
            VerifyRegistrationIdFormat(security.GetRegistrationID());

            Console.Write("ProvisioningClient RegisterAsync . . . ");
            DeviceRegistrationResult result = await provisioningDeviceClient.RegisterAsync().ConfigureAwait(false);

            Console.WriteLine($"{result.Status}");
            Console.WriteLine($"ProvisioningClient AssignedHub: {result.AssignedHub}; DeviceID: {result.DeviceId}");

            if (result.Status != ProvisioningRegistrationStatusType.Assigned) return;

            var auth = new DeviceAuthenticationWithRegistrySymmetricKey(result.DeviceId,
                (security as SecurityProviderSymmetricKey).GetPrimaryKey());

            using (DeviceClient iotClient = DeviceClient.Create(result.AssignedHub, auth, TransportType.Amqp))
            {
                Console.WriteLine("DeviceClient OpenAsync.");
                await iotClient.OpenAsync().ConfigureAwait(false);
                Console.WriteLine("DeviceClient SendEventAsync.");
                await iotClient.SendEventAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new
                {
                    Boiler1 = new Boiler()
                })))).ConfigureAwait(false);
                Console.WriteLine("DeviceClient CloseAsync.");
                await iotClient.CloseAsync().ConfigureAwait(false);
            }
        }

        private static void VerifyRegistrationIdFormat(string v)
        {
            var r = new Regex("^[a-z0-9-]*$");
            if (!r.IsMatch(v))
            {
                throw new FormatException("Invalid registrationId: The registration ID is alphanumeric, lowercase, and may contain hyphens");
            }
        }
    }
}