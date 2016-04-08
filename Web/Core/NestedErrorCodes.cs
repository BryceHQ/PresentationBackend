namespace Web.Core
{
    public static class NestedErrorCodes
    {
        public static ErrorCode NotAuthenticated = new ErrorCode(1000, "未验证用户");

        public static ErrorCode GenerateIdentityIconError= new ErrorCode(2000, "生成用户的个人头像出错");

    }
}