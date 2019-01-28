﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrossHMI.Interfaces.Networking;
using CrossHMI.Shared.Configuration;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Shared.Devices
{
    public class GenericDevice : NetworkDeviceBase
    {
        private readonly GenericDeviceConfiguration _genericDeviceConfiguration;
        public string Repository { get; private set; }
        public List<string> PropertiesNames { get; private set; }
        public Dictionary<string, object> Values { get; } = new Dictionary<string, object>();
        public Dictionary<string, Type> Properties { get; } = new Dictionary<string, Type>();

        public GenericDevice(GenericDeviceConfiguration genericDeviceConfiguration)
        {
            _genericDeviceConfiguration = genericDeviceConfiguration;
            PropertiesNames = _genericDeviceConfiguration.Properties.Keys.ToList();
            Properties =
                genericDeviceConfiguration.Properties.ToDictionary(pair => pair.Key, pair => ConvertToType(pair.Value));
        }

        public override void AssignRepository(string repository)
        {
            Repository = repository;
        }

        public override void ProcessPropertyUpdate<T>(string variableName, T value)
        {
            Values[variableName] = value;
            RaisePropertyChanged(variableName);
        }

        public override void DefineDevice<TConfiguration>(INetworkDeviceDefinitionBuilder<TConfiguration> builder)
        {
            foreach (var pair in Properties)
            {
                var method = builder
                    .GetType()
                    .GetMethod(nameof(builder.DefineVariable))?
                    .MakeGenericMethod(pair.Value);
                method?.Invoke(builder, new object[] {pair.Key});
            }
        }

        private Type ConvertToType(BuiltInType type)
        {
            switch (type)
            {
                case BuiltInType.Boolean:
                    return typeof(bool);
                case BuiltInType.SByte:
                    return typeof(sbyte);
                case BuiltInType.Byte:
                    return typeof(byte);
                case BuiltInType.Int16:
                    return typeof(short);
                case BuiltInType.UInt16:
                    return typeof(ushort);
                case BuiltInType.Int32:
                    return typeof(int);
                case BuiltInType.UInt32:
                    return typeof(uint);
                case BuiltInType.Int64:
                    return typeof(long);
                case BuiltInType.UInt64:
                    return typeof(ulong);
                case BuiltInType.Float:
                    return typeof(float);
                case BuiltInType.Double:
                    return typeof(double);
                case BuiltInType.String:
                    return typeof(string);
                case BuiltInType.DateTime:
                    return typeof(DateTime);
                case BuiltInType.Guid:
                    return typeof(Guid);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
