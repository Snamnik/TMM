namespace TMM.Domain.Specifications
{
    //Keyset pagination- The recommended alternative to offset-based pagination.
    //https://learn.microsoft.com/en-us/ef/core/querying/pagination

    public class CustomersSpec : Specification<Customer>
    {
        public CustomersSpec(int lastCustomerId, int size)
        {
            Query.OrderBy(c => c.Id).Where(c => c.Id > lastCustomerId).Take(size);
        }
    }
    public class ActiveCustomersSpec : Specification<Customer>
    {
        public ActiveCustomersSpec(int lastCustomerId, int size)
        {
            Query.OrderBy(c => c.Id).Where(c => c.Id > lastCustomerId && c.IsActive).Take(size);
        }
    }
    public class InactiveCustomersSpec : Specification<Customer>
    {
        public InactiveCustomersSpec(int lastCustomerId, int size)
        {
            Query.OrderBy(c => c.Id).Where(c => c.Id > lastCustomerId && !c.IsActive).Take(size);
        }
    }
}
