using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace presentation.Business
{
    public static class Helper
    {
        public static int ToInt(string value)
        {
            try
            {
                return Int32.Parse(value);
            }
            catch (FormatException)
            {
                return 0;
            }
        }
    }
}