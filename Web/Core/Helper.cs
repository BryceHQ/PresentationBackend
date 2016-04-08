using System;

namespace Web.Core
{
    public static class Helper
    {
        public static int ToInt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }
            try
            {
                return Int32.Parse(value);
            }
            catch (FormatException)
            {
                return 0;
            }
        }

        public static string Trim(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            return value.Trim();
        }
    }
}
