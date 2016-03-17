using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core;

namespace Business
{
    public static class NestedErrorCodes
    {
        public static ErrorCode NotAuthenticated = new ErrorCode(1000, "未验证用户");
    }
}
