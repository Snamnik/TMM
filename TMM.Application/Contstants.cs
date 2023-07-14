namespace TMM.Application
{
    public static class ExpressionPattern
    {
        public const string MobileNo = "^(\\+|[0-9])\\d{5,14}";
    }


    public static class TMMEventId
    {
        public const int BadRequest = 1001;
        public const int InternalServerError = 1002;
    }
}
