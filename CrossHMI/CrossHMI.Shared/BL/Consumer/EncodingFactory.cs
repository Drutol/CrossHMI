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

        class Decoder : UABinaryDecoder
        {
            public override IDataValue ReadDataValue(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public override IDiagnosticInfo ReadDiagnosticInfo(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public override IExpandedNodeId ReadExpandedNodeId(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public override IExtensionObject ReadExtensionObject(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public override ILocalizedText ReadLocalizedText(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public override INodeId ReadNodeId(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public override IQualifiedName ReadQualifiedName(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public override XmlElement ReadXmlElement(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }

            public override IStatusCode ReadStatusCode(IBinaryDecoder decoder)
            {
                throw new NotImplementedException();
            }
        }

        class Encoder : UABinaryEncoder
        {
            public override void Write(IBinaryEncoder encoder, IDataValue value)
            {
                throw new NotImplementedException();
            }

            public override void Write(IBinaryEncoder encoder, IDiagnosticInfo value)
            {
                throw new NotImplementedException();
            }

            public override void Write(IBinaryEncoder encoder, IExpandedNodeId value)
            {
                throw new NotImplementedException();
            }

            public override void Write(IBinaryEncoder encoder, IExtensionObject value)
            {
                throw new NotImplementedException();
            }

            public override void Write(IBinaryEncoder encoder, ILocalizedText value)
            {
                throw new NotImplementedException();
            }

            public override void Write(IBinaryEncoder encoder, INodeId value)
            {
                throw new NotImplementedException();
            }

            public override void Write(IBinaryEncoder encoder, IQualifiedName value)
            {
                throw new NotImplementedException();
            }

            public override void Write(IBinaryEncoder encoder, XmlElement value)
            {
                throw new NotImplementedException();
            }

            public override void Write(IBinaryEncoder encoder, IStatusCode value)
            {
                throw new NotImplementedException();
            }
        }
    }
}
