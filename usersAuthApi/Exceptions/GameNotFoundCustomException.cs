namespace usersAuthApi.Exceptions
{
    public class GameNotFoundCustomException : Exception
    {
        public GameNotFoundCustomException(string message) : base(message) { }
        public GameNotFoundCustomException(string message, Exception innerException) : base(message, innerException) { }
    }
}



