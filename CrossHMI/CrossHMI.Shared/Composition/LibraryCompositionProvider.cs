using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;
using System.Text;
using CrossHMI.Interfaces;

namespace CrossHMI.Shared.Composition
{
    public class LibraryCompositionProvider : ILibraryCompositionProvider
    {
        private readonly CompositionContainer _container;

        public LibraryCompositionProvider()
        {
            _container = new CompositionContainer(new AggregateCatalog(new AssemblyCatalog(Assembly.GetAssembly(typeof(int)))));
        }

        public T ObtainExport<T>()
        {
            return _container.GetExportedValue<T>();
        }

        public IEnumerable<T> ObtainExports<T>()
        {
            return _container.GetExportedValues<T>();
        }
    }
}
