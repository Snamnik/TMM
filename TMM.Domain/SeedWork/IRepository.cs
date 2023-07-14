public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}

public interface IReadOnlyRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}
