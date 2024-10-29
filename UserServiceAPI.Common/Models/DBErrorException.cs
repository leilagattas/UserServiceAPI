namespace UserServiceAPI.Common.Models
{
    public class DBErrorException : Exception
    {
        public DBErrorException() { }

        public DBErrorException(string message) : base(message) { }

        public DBErrorException(string message, Exception inner) : base(message, inner) { }
    }
}
