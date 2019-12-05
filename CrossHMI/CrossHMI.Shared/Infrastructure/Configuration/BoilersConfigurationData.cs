using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Shared.Infrastructure.Configuration
{
    /// <summary>
    ///     Extended <see cref="ConfigurationData" /> with list of <see cref="BoilerRepositoryDetails" />.
    /// </summary>
    public class BoilersConfigurationData : ConfigurationData
    {
        /// <summary>
        ///     Gets the list of all details found in configuration file.
        /// </summary>
        [DataMember]
        public List<BoilerRepositoryDetails> AdditionalRepositoryData { get; set; }
    }
}