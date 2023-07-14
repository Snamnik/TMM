namespace TMM.Domain.Exceptions
{
    public class CustomerIsInactiveException : TMMException
    {
        public CustomerIsInactiveException(int customerId) :
            base($"a customer with id {customerId} is inactive.")
        { }
    }
}
