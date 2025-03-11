namespace VotingApp.API.Exceptions
{
    public class UnauthorizedAccessException:Exception
    {
        public UnauthorizedAccessException(String message) : base(message) { }
    }
}
