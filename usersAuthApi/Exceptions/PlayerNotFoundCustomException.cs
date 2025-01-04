namespace usersAuthApi.Exceptions
{
    public class PlayerNotFoundCustomException : Exception
    {
        public PlayerNotFoundCustomException() : base() { }
        public PlayerNotFoundCustomException(string message) : base(message) { }
        public PlayerNotFoundCustomException(string message, Exception innerException) : base(message, innerException) { }
    }
}



