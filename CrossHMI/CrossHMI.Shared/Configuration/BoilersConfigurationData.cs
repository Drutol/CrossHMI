using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Shared.Configuration
{
    [DataContract(Name = "ConfigurationData", Namespace = "http://commsvr.com/UAOOI/SemanticData/UANetworking/Configuration/Serialization.xsd")]
    [XmlRoot(Namespace = "http://commsvr.com/UAOOI/SemanticData/UANetworking/Configuration/Serialization.xsd")]
    public class BoilersConfigurationData : ConfigurationData
    {
        private string _repositoryExtensions;

        [IgnoreDataMember]
        public List<BoilerRepositoryDetails> RepositoriesDetails { get; set; }

        [DataMember]
        private string RepositoryExtensions
        {
            get => _repositoryExtensions;
            set
            {
                _repositoryExtensions = value;
                RepositoriesDetails = JsonConvert.DeserializeObject<List<BoilerRepositoryDetails>>(value);
            }
        }
    }
}
