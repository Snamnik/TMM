using Ardalis.Specification.EntityFrameworkCore;
using TMM.Domain.SeedWork;

namespace TMM.Infrastructure.Data
{
    public class TMMRepository<T> : RepositoryBase<T>, IReadOnlyRepository<T>, IRepository<T> where T : class, IAggregateRoot
    {
        private readonly TMMDbContext dbContext;

        public TMMRepository(TMMDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}