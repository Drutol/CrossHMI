using System;
using System.Collections.Generic;
using System.Text;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Interfaces
{
    /// <summary>
    /// Interface for objects that are found in extended <see cref="ConfigurationData"/>.
    /// </summary>
    public interface IAdditonalRepositoryDataDescriptor
    {
        /// <summary>
        /// The repository that this additional data is targetting.
        /// </summary>
        string Repository { get; }
    }
}
