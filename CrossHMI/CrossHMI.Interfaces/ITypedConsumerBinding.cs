using System;
using System.Collections.Generic;
using System.Text;

namespace CrossHMI.Interfaces
{
    public interface ITypedConsumerBinding<T>
    {
        string RepositoryName { get; set; }
        string VariableName { get; set; }
        T Value { get; set; }
    }
}
