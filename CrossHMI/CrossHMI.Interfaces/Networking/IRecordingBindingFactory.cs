using System;
using System.Collections.Generic;
using CrossHMI.Models.Networking;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.SemanticData.DataRepository;

namespace CrossHMI.Interfaces.Networking
{
    /// <summary>
    ///     Extension to <see cref="IBindingFactory" /> exposing <see cref="IConsumerBinding" /> per every repository.
    /// </summary>
    public interface IRecordingBindingFactory : IBindingFactory
    {
        /// <summary>
        /// Called whenever new binding is about to be created for new repository.
        /// </summary>
        event EventHandler<string> NewRepositoryReceived;

        /// <summary>
        /// Called whenever new binding gets created providing repository name.
        /// </summary>
        event EventHandler<CreateBindingEventArgs> NewBindingCreated;

        /// <summary>
        ///     Gets the dictionary with all bindings created for repository.
        /// </summary>
        /// <param name="repository">The name of repository we want to get bindings of.</param>
        /// <returns>Dictionary with pairs of variable name and related <see cref="IConsumerBinding" />.</returns>
        Dictionary<string, IConsumerBinding> GetConsumerBindingsForRepository(string repository);
    }
}