namespace Web.Core
{
    /// <summary>
    /// 对实体层返回的结果的包装，包括异常。
    /// </summary>
    /// <typeparam name="T">返回结果的类型</typeparam>
    public class ReturnValue<T>
    {
        private bool successed;

        public bool Successed
        {
            get { return this.successed; }
        }

        private T value;

        public T Value
        {
            get { return this.value; }
        }

        private ErrorCode errorCode;

        public ErrorCode ErrorCode
        {
            get { return this.errorCode; }
        }


        public ReturnValue(T value, ErrorCode errorCode)
        {
            this.successed = errorCode == null;
            this.value = value;
            this.errorCode = errorCode;
        }

        public ReturnValue(T value)
            : this(true, value, null)
        {
        }

        public ReturnValue(bool successed, T value)
            : this(successed, value, null)
        {
        }

        public ReturnValue(bool successed, T value, ErrorCode errorCode)
        {
            this.successed = successed;
            this.value = value;
            this.errorCode = errorCode;
        }
    }
}
