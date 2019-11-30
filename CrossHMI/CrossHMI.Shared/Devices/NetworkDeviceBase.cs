using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CrossHMI.Interfaces.Networking;
using GalaSoft.MvvmLight;
using UAOOI.Configuration.Networking.Serialization;

namespace CrossHMI.Shared.Devices
{
    /// <summary>
    ///     Base utility class for final model classes. Provides functionality of automatically defining marked properties.
    /// </summary>
    public abstract class NetworkDeviceBase : ViewModelBase, INetworkDevice
    {
        private readonly
            Dictionary<Type, Dictionary<string, (ProcessVariableAttribute Attribute, PropertyInfo Property)>>
            _propertyMappings =
                new Dictionary<Type, Dictionary<string, (ProcessVariableAttribute Attribute, PropertyInfo Property)>>();
        
        /// <inheritdoc />
        public abstract string Repository { get; set; }

        /// <summary>
        ///     Base constructor that via reflection scans all properties for <see cref="ProcessVariableAttribute" />
        ///     and build a map of them to automate the definition process.
        /// </summary>
        protected NetworkDeviceBase()
        {
            var type = GetType();

            foreach (var property in type.GetProperties().Where(info =>
                info.CustomAttributes.Any(data => data.AttributeType == typeof(ProcessVariableAttribute))))
            {
                var attr = property.GetCustomAttribute<ProcessVariableAttribute>();

                if (!_propertyMappings.ContainsKey(property.PropertyType))
                    _propertyMappings[property.PropertyType] =
                        new Dictionary<string, (ProcessVariableAttribute attribute, PropertyInfo property)>();

                _propertyMappings[property.PropertyType][attr.ConfigurationPropertyName] = (attr, property);
            }
        }

        /// <inheritdoc />
        public virtual void ProcessPropertyUpdate<T>(string variableName, T value)
        {
            if (_propertyMappings.ContainsKey(typeof(T)) && _propertyMappings[typeof(T)].ContainsKey(variableName))
            {
                var mapping = _propertyMappings[typeof(T)][variableName];

                if (!mapping.Attribute.AutoDefine)
                    return;

                mapping.Property.SetValue(this, value);
                if (mapping.Attribute.RaisePropertyChanged)
                    RaisePropertyChanged(mapping.Property.Name);
            }
        }

        /// <inheritdoc />
        public virtual void DefineDevice(INetworkDeviceDefinitionBuilder builder)
        {
            foreach (var typeMappings in _propertyMappings)
            {
                var method = builder
                    .GetType()
                    .GetMethod(nameof(builder.DefineVariable))?
                    .MakeGenericMethod(typeMappings.Key);
                foreach (var propertyMapping in typeMappings.Value)
                    if (propertyMapping.Value.Attribute.AutoDefine)
                        method?.Invoke(builder,
                            new object[] {propertyMapping.Value.Attribute.ConfigurationPropertyName});
            }
        }

        /// <summary>
        ///     Used as a marker for variables that are meant to be used in definition process.
        /// </summary>
        [AttributeUsage(AttributeTargets.Property)]
        public class ProcessVariableAttribute : Attribute
        {
            /// <summary>
            ///     Initializes new <see cref="ProcessVariableAttribute" />
            ///     with default configuration variable name being the one used by the actual caller property.
            /// </summary>
            /// <param name="configurationPropertyName">The name of the variable found in configuration file.</param>
            public ProcessVariableAttribute([CallerMemberName] string configurationPropertyName = null)
            {
                ConfigurationPropertyName = configurationPropertyName;
            }

            /// <summary>
            ///     The name of the variable defined in configuration.
            /// </summary>
            public string ConfigurationPropertyName { get; }

            /// <summary>
            ///     Determines whether this variable should be automatically defined in
            ///     <see cref="INetworkDeviceDefinitionBuilder" />
            /// </summary>
            public bool AutoDefine { get; set; } = true;

            /// <summary>
            ///     Determines wheter newly received values should trigger <see cref="ObservableObject.PropertyChanged" />
            /// </summary>
            public bool RaisePropertyChanged { get; set; } = true;
        }
    }
}