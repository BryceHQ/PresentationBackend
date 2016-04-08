using System;
using System.Collections.Generic;

namespace Web.Core
{
    public class DefaultAsyncResult<T> : IAsyncResult<T> where T : class
    {
        #region Constructor
        /// <summary>
        ///     Failure constructor that takes error messages
        /// </summary>
        /// <param name="errors"></param>
        public DefaultAsyncResult(params ErrorCode[] errors)
            : this((IEnumerable<ErrorCode>)errors)
        {
        }

        public DefaultAsyncResult(IEnumerable<ErrorCode> errors)
            : this(false, null, errors ?? new ErrorCode[] { new ErrorCode("未知错误") })
        {
        }

        public DefaultAsyncResult(T value)
            : this(true, value, null)
        {
        }

        public DefaultAsyncResult(T value, ErrorCode error)
            : this(value, error == null ? null : new ErrorCode[] { error })
        {
        }

        public DefaultAsyncResult(T value, IEnumerable<ErrorCode> errors)
            : this(errors == null, value, errors)
        {
        }

        /// <summary>
        /// Constructor that takes whether the result is successful
        /// </summary>
        /// <param name="success"></param>
        /// <param name="value"></param>
        protected DefaultAsyncResult(bool success, T value, IEnumerable<ErrorCode> errors)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            Succeeded = success;
            Errors = errors;
            Value = value;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Value
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        ///     True if the operation was successful
        /// </summary>
        public bool Succeeded { get; private set; }

        /// <summary>
        ///     List of errors
        /// </summary>
        public IEnumerable<ErrorCode> Errors { get; private set; }

        #endregion

        #region Static
        /// <summary>
        ///     Static success result
        /// </summary>
        /// <returns></returns>
        public static DefaultAsyncResult<T> Successed(T value)
        {
            return new DefaultAsyncResult<T>(value);
        }

        /// <summary>
        ///     Failed helper method
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static DefaultAsyncResult<T> Failed(params ErrorCode[] errors)
        {
            return new DefaultAsyncResult<T>(errors);
        } 
        #endregion
    }
}