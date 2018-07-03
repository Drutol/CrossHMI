using CrossHMI.Interfaces;

namespace CrossHMI.Shared.Variables
{
    public class VariableUpdatedEntry : INetworkVariable
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public void Initialize(string raw)
        {
            Value = raw;
        }
    }
}
