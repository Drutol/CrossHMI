using System;
using System.Collections.Generic;
using CrossHMI.LibraryIntegration.Interfaces;
using CrossHMI.LibraryIntegration.Models;
using UAOOI.Configuration.Networking.Serialization;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.SemanticData.DataRepository;

namespace CrossHMI.LibraryIntegration.Infrastructure
{
    /// <summary>
    ///     Class responsible for creating bindings for every variable in every repository as requested by library.
    /// </summary>
    public class ConsumerBindingFactory : IRecordingBindingFactory
    {
        private readonly Dictionary<string, Dictionary<string, IConsumerBinding>> _consumerBindings =
            new Dictionary<string, Dictionary<string, IConsumerBinding>>();

        /// <inheritdoc />
        public event EventHandler<string> NewRepositoryReceived;     
        
        /// <inheritdoc />
        public event EventHandler<CreateBindingEventArgs> NewBindingCreated;

        /// <inheritdoc />
        IConsumerBinding IBindingFactory.GetConsumerBinding(
            string repositoryGroup,
            string processValueName,
            UATypeInfo fieldTypeInfo)
        {
            NewRepositoryReceived?.Invoke(this, repositoryGroup);
            var (binding, type) = GetConsumerBinding(repositoryGroup, processValueName, fieldTypeInfo);
            NewBindingCreated?.Invoke(this, new CreateBindingEventArgs(repositoryGroup, processValueName, type));
            return binding;
        }

        /// <inheritdoc />
        IProducerBinding IBindingFactory.GetProducerBinding(
            string repository,
            string processValueName,
            UATypeInfo fieldTypeInfo)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Dictionary<string, IConsumerBinding> GetConsumerBindingsForRepository(string repository)
        {
            if (_consumerBindings.ContainsKey(repository))
                return _consumerBindings[repository];

            throw new ArgumentOutOfRangeException(nameof(repository), $"Unknown repository \"{repository}\"");
        }

        private (IConsumerBinding Binding, Type BindingType) GetConsumerBinding(string repositoryGroup, string variableName, UATypeInfo typeInfo)
        {
            if (typeInfo.ValueRank == 0 || typeInfo.ValueRank > 1)
                throw new ArgumentOutOfRangeException(nameof(typeInfo.ValueRank));
            switch (typeInfo.BuiltInType)
            {
                case BuiltInType.Boolean:
                    return typeInfo.ValueRank < 0 ? AddBinding<bool>() : AddBinding<bool[]>();
                case BuiltInType.SByte:
                    return typeInfo.ValueRank < 0 ? AddBinding<sbyte>() : AddBinding<sbyte[]>();
                case BuiltInType.Byte:
                    return typeInfo.ValueRank < 0 ? AddBinding<byte>() : AddBinding<byte[]>();
                case BuiltInType.Int16:
                    return typeInfo.ValueRank < 0 ? AddBinding<short>() : AddBinding<short[]>();
                case BuiltInType.UInt16:
                    return typeInfo.ValueRank < 0 ? AddBinding<ushort>() : AddBinding<ushort[]>();
                case BuiltInType.Int32:
                    return typeInfo.ValueRank < 0 ? AddBinding<int>() : AddBinding<int[]>();
                case BuiltInType.UInt32:
                    return typeInfo.ValueRank < 0 ? AddBinding<uint>() : AddBinding<uint[]>();
                case BuiltInType.Int64:
                    return typeInfo.ValueRank < 0 ? AddBinding<long>() : AddBinding<long[]>();
                case BuiltInType.UInt64:
                    return typeInfo.ValueRank < 0 ? AddBinding<ulong>() : AddBinding<ulong[]>();
                case BuiltInType.Float:
                    return typeInfo.ValueRank < 0 ? AddBinding<float>() : AddBinding<float[]>();
                case BuiltInType.Double:
                    return typeInfo.ValueRank < 0 ? AddBinding<double>() : AddBinding<double[]>();
                case BuiltInType.String:
                    return typeInfo.ValueRank < 0 ? AddBinding<string>() : AddBinding<string[]>();
                case BuiltInType.DateTime:
                    return typeInfo.ValueRank < 0 ? AddBinding<DateTime>() : AddBinding<DateTime[]>();
                case BuiltInType.Guid:
                    return typeInfo.ValueRank < 0 ? AddBinding<Guid>() : AddBinding<Guid[]>();
                case BuiltInType.ByteString:
                    return typeInfo.ValueRank < 0 ? AddBinding<byte[]>() : AddBinding<byte[][]>();
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeInfo.BuiltInType));
            }

            (IConsumerBinding Binding, Type BindingType) AddBinding<T>()
            {
                return (this.AddBinding<T>(repositoryGroup, variableName, typeInfo), typeof(T));
            }
        }

        private IConsumerBinding AddBinding<T>(string repositoryGroup, string variableName, UATypeInfo typeInfo)
        {
            var monitoredValue = new ConsumerBindingMonitoredValue<T>(typeInfo);
            if (!_consumerBindings.ContainsKey(repositoryGroup))
                _consumerBindings[repositoryGroup] = new Dictionary<string, IConsumerBinding>();
            _consumerBindings[repositoryGroup][variableName] = monitoredValue;
            return monitoredValue;
        }
    }
}