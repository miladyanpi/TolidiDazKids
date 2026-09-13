namespace TolidiAyhan.Exceptions
{
    public class UnauthenticatedException : Exception
    {
        public UnauthenticatedException() : base("کاربر احراز هویت نشده است") { }
    }
}
