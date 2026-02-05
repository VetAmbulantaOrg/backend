namespace Exam.App.Services.Exceptions
{
    public class OwnerNotFound : Exception
    {
        public OwnerNotFound(string name) : base($"User with username {name} could not be found.")
        {
        }
    }
}
