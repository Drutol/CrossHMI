using System;
using System.Collections.Generic;
using System.Text;
using UAOOI.Configuration.Networking.Serialization;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.SemanticData.DataRepository;

namespace CrossHMI.Shared.BL.Consumer
{
    public class ConsumerBindingFactory : IBindingFactory
    {
        /// <summary>
        /// Gets the binding captured by an instance of the <see cref="IConsumerBinding" /> type used by the consumer to save the data in the data repository.
        /// </summary>
        IConsumerBinding IBindingFactory.GetConsumerBinding(string repositoryGroup, string processValueName, UATypeInfo fieldTypeInfo)
        {
            //if (repositoryGroup != Encoding.EncodingCompositionSettings.ConfigurationRepositoryGroup)
            //    throw new ArgumentNullException("repositoryGroup");
            return GetConsumerBinding(processValueName, fieldTypeInfo);
        }
        /// <summary>
        /// Gets the producer binding.
        /// </summary>
        IProducerBinding IBindingFactory.GetProducerBinding(string repositoryGroup, string processValueName, UATypeInfo fieldTypeInfo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Helper method that creates the consumer binding.
        /// </summary>
        private IConsumerBinding GetConsumerBinding(string variableName, UATypeInfo typeInfo)
        {
            IConsumerBinding _return;
            if (typeInfo.ValueRank == 0 || typeInfo.ValueRank > 1)
                throw new ArgumentOutOfRangeException(nameof(typeInfo.ValueRank));
            switch (typeInfo.BuiltInType)
            {
                case BuiltInType.Boolean:
                    if (typeInfo.ValueRank < 0)
                        _return = AddBinding<bool>(variableName, typeInfo);
                    else
                        _return = AddBinding<bool[]>(variableName, typeInfo);
                    break;
                case BuiltInType.SByte:
                    if (typeInfo.ValueRank < 0)
                        _return = AddBinding<sbyte>(variableName, typeInfo);
                    else
                        _return = AddBinding<sbyte[]>(variableName, typeInfo);
                    break;
                case BuiltInType.Byte:
                    if (typeInfo.ValueRank < 0)
                        _return = AddBinding<byte>(variableName, typeInfo);
                    else
                        _return = AddBinding<byte[]>(variableName, typeInfo);
                    break;
                case BuiltInType.Int16:
                    if (typeInfo.ValueRank < 0)
                        _return = AddBinding<short>(variableName, typeInfo);
                    else
                        _return = AddBinding<short[]>(variableName, typeInfo);
                    break;
                case BuiltInType.UInt16:
                    if (typeInfo.ValueRank < 0)
                        _return = AddBinding<ushort>(variableName, typeInfo);
                    else
                        _return = AddBinding<ushort[]>(variableName, typeInfo);
                    break;
                case BuiltInType.Int32:
                    if (typeInfo.ValueRank < 0)
                        _return = AddBinding<int>(variableName, typeInfo);
                    else
                        _return = AddBinding<int[]>(variableName, typeInfo);
                    break;
                case BuiltInType.UInt32:
                    if (typeInfo.ValueRank < 0)
                        _return = AddBinding<uint>(variableName, typeInfo);
                    else
                        _return = AddBinding<uint[]>(variableName, typeInfo);
                    break;
                case BuiltInType.Int64:
                    if (typeInfo.ValueRank < 0)
                        _return = AddBinding<long>(variableName, typeInfo);
                    else
                        _return = AddBinding<long[]>(variableName, typeInfo);
                    break;
                case BuiltInType.UInt64:
                    if (typeInfo.ValueRank < 0)
                        _return = AddBinding<ulong>(variableName, typeInfo);
                    else
                        _return = AddBinding<ulong[]>(variableName, typeInfo);
                    break;
                case BuiltInType.Float:
                    if (typeInfo.ValueRank < 0)
                        _return = AddBinding<float>(variableName, typeInfo);
                    else
                        _return = AddBinding<float[]>(variableName, typeInfo);
                    break;
                case BuiltInType.Double:
                    if (typeInfo.ValueRank < 0)
                        _return = AddBinding<double>(variableName, typeInfo);
                    else
                        _return = AddBinding<double[]>(variableName, typeInfo);
                    break;
                case BuiltInType.String:
                    if (typeInfo.ValueRank < 0)
                        _return = AddBinding<string>(variableName, typeInfo);
                    else
                        _return = AddBinding<string[]>(variableName, typeInfo);
                    break;
                case BuiltInType.DateTime:
                    if (typeInfo.ValueRank < 0)
                        _return = AddBinding<DateTime>(variableName, typeInfo);
                    else
                        _return = AddBinding<DateTime[]>(variableName, typeInfo);
                    break;
                case BuiltInType.Guid:
                    if (typeInfo.ValueRank < 0)
                        _return = AddBinding<Guid>(variableName, typeInfo);
                    else
                        _return = AddBinding<Guid[]>(variableName, typeInfo);
                    break;
                case BuiltInType.ByteString:
                    if (typeInfo.ValueRank < 0)
                        _return = AddBinding<byte[]>(variableName, typeInfo);
                    else
                        _return = AddBinding<byte[][]>(variableName, typeInfo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(typeInfo.BuiltInType));
            }
            return _return;
        }

        private IConsumerBinding AddBinding<T>(string variableName, UATypeInfo typeInfo)
        {
            return new ConsumerBindingMonitoredValue<T>(typeInfo);
        }
    }
}
