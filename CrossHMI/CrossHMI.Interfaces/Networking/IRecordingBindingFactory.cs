using System.Collections.Generic;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.SemanticData.DataRepository;

namespace CrossHMI.Interfaces.Networking
{
    /// <summary>
    /// Extension to <see cref="IBindingFactory"/> exposing <see cref="IConsumerBinding"/> per every repository.
    /// </summary>
    public interface IRecordingBindingFactory : IBindingFactory
    {
        /// <summary>
        /// Gets the dictionary with all bindings created for repository.
        /// </summary>
        /// <param name="repository">The name of repository we want to get bindings of.</param>
        /// <returns>Dictionary with pairs of variable name and related <see cref="IConsumerBinding"/>.</returns>
        Dictionary<string, IConsumerBinding> GetConsumerBindingsForRepository(string repository);
    }
}
