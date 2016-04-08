using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core
{
    /// <summary>
    /// an async result for entity operation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAsyncResult<T> where T : class
    {
        /// <summary>
        ///     True if the operation was successful
        /// </summary>
        bool Succeeded { get; }

        /// <summary>
        ///     List of errors
        /// </summary>
        IEnumerable<ErrorCode> Errors { get; }

        T Value { get; }
    }
}
