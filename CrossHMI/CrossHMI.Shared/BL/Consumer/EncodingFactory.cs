using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UAOOI.Configuration.Networking.Serialization;
using UAOOI.Networking.Encoding;
using UAOOI.Networking.SemanticData;
using UAOOI.Networking.SemanticData.DataRepository;
using UAOOI.Networking.SemanticData.Encoding;

namespace CrossHMI.Shared.BL.Consumer
{
    public class EncodingFactory : IEncodingFactory
    {
        void IEncodingFactory.UpdateValueConverter(IBinding binding, string repositoryGroup, UATypeInfo sourceEncoding)
        {

        }

        public IUADecoder UADecoder { get; } = new Decoder();

        public IUAEncoder UAEncoder { get; } = new Encoder();

        class Decoder : IUADecoder
        {
            public Guid ReadGuid(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public DateTime ReadDateTime(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public byte[] ReadByteString(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public string ReadString(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public IDataValue ReadDataValue(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public IDiagnosticInfo ReadDiagnosticInfo(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public IExpandedNodeId ReadExpandedNodeId(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public IExtensionObject ReadExtensionObject(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public ILocalizedText ReadLocalizedText(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public INodeId ReadNodeId(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public IQualifiedName ReadQualifiedName(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public XmlElement ReadXmlElement(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public IStatusCode ReadStatusCode(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public IVariant ReadVariant(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public Array ReadArray<type>(IBinaryDecoder decoder, Func<type> readValue, bool arrayDimensionsPresents)
            {
                throw new NotImplementedException();
            }
        }

        class Encoder : IUAEncoder
        {
            public void Write(IBinaryEncoder encoder, DateTime value)
            {
                throw new NotImplementedException();
            }

            public void Write(IBinaryEncoder encoder, byte[] value)
            {
                throw new NotImplementedException();
            }

            public void Write(IBinaryEncoder encoder, IDataValue value)
            {
                throw new NotImplementedException();
            }

            public void Write(IBinaryEncoder encoder, IDiagnosticInfo value)
            {
                throw new NotImplementedException();
            }

            public void Write(IBinaryEncoder encoder, IExpandedNodeId value)
            {
                throw new NotImplementedException();
            }

            public void Write(IBinaryEncoder encoder, IExtensionObject value)
            {
                throw new NotImplementedException();
            }

            public void Write(IBinaryEncoder encoder, ILocalizedText value)
            {
                throw new NotImplementedException();
            }

            public void Write(IBinaryEncoder encoder, INodeId value)
            {
                throw new NotImplementedException();
            }

            public void Write(IBinaryEncoder encoder, IQualifiedName value)
            {
                throw new NotImplementedException();
            }

            public void Write(IBinaryEncoder encoder, XmlElement value)
            {
                throw new NotImplementedException();
            }

            public void Write(IBinaryEncoder encoder, IStatusCode value)
            {
                throw new NotImplementedException();
            }

            public void Write(IBinaryEncoder encoder, IVariant value)
            {
                throw new NotImplementedException();
            }

            public void Write(IBinaryEncoder encoder, Guid value)
            {
                throw new NotImplementedException();
            }

            public void Write(IBinaryEncoder encoder, string value)
            {
                throw new NotImplementedException();
            }

            public void WriteArray<type>(IBinaryEncoder encoder, Array value, Action<type> writeValue, BuiltInType builtInType)
            {
                throw new NotImplementedException();
            }
        }
    }
}
