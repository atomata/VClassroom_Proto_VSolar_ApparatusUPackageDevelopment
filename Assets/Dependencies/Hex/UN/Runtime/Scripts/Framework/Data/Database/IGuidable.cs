using System;

namespace HexUN.Data
{
    public interface IGuidable
    {
        /// <summary>
        /// The Guid id of the object 
        /// </summary>
        Guid Guid { get; }
    }
}
