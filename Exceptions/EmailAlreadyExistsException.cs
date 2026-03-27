namespace WebApplication8.Exceptions
{
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException(string email)
                        :base($"Пользователь с таким Email'{email}' уже существует.")
        {

        }
    }
}
