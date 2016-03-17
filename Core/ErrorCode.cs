using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class ErrorCode
    {
        #region fields

        private int code; //从1000开始
        private string description;

        #endregion

        #region properties

        public int Code
        {
            get
            {
                return code;
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
        }

        #endregion

        #region constructor
        public ErrorCode()
            : this(10000, "unkown error.")
        {

        }
        public ErrorCode(string description)
            : this(10000, description)
        {

        }
        public ErrorCode(int code, string description)
        {
            this.code = code;
            this.description = description;
        }
        #endregion
    }

}
