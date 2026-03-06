namespace Exam.App.Services.Exceptions
{
    public class OwnerNotFound : Exception
    {
        public OwnerNotFound(int id) : base($"User with ID {id} could not be found.")
        {
        }
    }
}
