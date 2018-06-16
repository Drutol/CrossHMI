using System;
using System.Collections.Generic;
using System.Text;

namespace CrossHMI.Interfaces
{
    public interface ILibraryCompositionProvider
    {
        T ObtainExport<T>();
        IEnumerable<T> ObtainExports<T>();
    }
}
