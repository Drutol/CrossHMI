using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using CrossHMI.Interfaces.Networking;
using GalaSoft.MvvmLight;

namespace CrossHMI.Shared.Devices
{
    public abstract class NetworkDeviceBase : ViewModelBase, INetworkDevice
    {
        protected class AutoMapAttribute : Attribute
        {
            public AutoMapAttribute([CallerMemberName] string configurationPropertyName = null)
            {
                ConfigurationPropertyName = configurationPropertyName;
            }

            public string ConfigurationPropertyName { get; set; }
            public bool AutoDefine { get; set; } = true;
            public bool RaisePropertyChanged { get; set; } = true;
        }

        private Dictionary<Type, Dictionary<string, (AutoMapAttribute Attribute,PropertyInfo Property)>> _propertyMappings =
            new Dictionary<Type, Dictionary<string, (AutoMapAttribute Attribute, PropertyInfo Property)>>();

        public NetworkDeviceBase()
        {
            var type = this.GetType();

            foreach (var property in type.GetProperties().Where(info =>
                info.CustomAttributes.Any(data => data.AttributeType == typeof(AutoMapAttribute))))
            {
                var attr = property.GetCustomAttribute<AutoMapAttribute>();

                if(!_propertyMappings.ContainsKey(property.PropertyType))
                    _propertyMappings[property.PropertyType] = new Dictionary<string, (AutoMapAttribute attribute, PropertyInfo property)>();

                _propertyMappings[property.PropertyType][attr.ConfigurationPropertyName] = (attr,property);
            }
        }

        public abstract void AssignRepository(string repository);

        public virtual void ProcessPropertyUpdate<T>(string variableName, T value)
        {
            if (_propertyMappings.ContainsKey(typeof(T)) && _propertyMappings[typeof(T)].ContainsKey(variableName))
            {
                var mapping = _propertyMappings[typeof(T)][variableName];
                                      
                mapping.Property.SetValue(this, value);
                if(mapping.Attribute.RaisePropertyChanged)
                    RaisePropertyChanged(mapping.Property.Name);
            }           
        }

        public virtual void DefineVariables(INetworkDeviceDefinitionBuilder builder)
        {
            foreach (var typeMappings in _propertyMappings)
            {
                var method = builder
                    .GetType()
                    .GetMethod(nameof(builder.Define))?
                    .MakeGenericMethod(typeMappings.Key);
                foreach (var propertyMapping in typeMappings.Value)
                {
                    if (propertyMapping.Value.Attribute.AutoDefine)
                    {
                        method?.Invoke(builder, new object[] {propertyMapping.Value.Attribute.ConfigurationPropertyName});
                    }
                }
            }
        }
    }
}
