namespace TMM.Domain.Exceptions
{
    public class CustomerDoesNotExistException : TMMException
    {
        public CustomerDoesNotExistException(int customerId) :
            base($"a customer with id {customerId} doesn't exist")
        { }
    }
}
