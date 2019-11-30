using System;
using System.Collections.Generic;
using System.Text;

namespace CrossHMI.Interfaces.Networking
{
    /// <summary>
    ///     Interface for objects representing registration of an extension.
    /// </summary>
    public interface IExtensionDeclaration
    {
        /// <summary>
        ///     Used to find proper extension object and pass
        ///     it to underlying device via provided callback delegate.
        /// </summary>
        void Assign();
    }
}
