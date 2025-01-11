using Ardalis.Specification;
using BookWorm.SharedKernel.Core.Model;

namespace BookWorm.SharedKernel.Repositories;

public interface IReadRepository<T> : IReadRepositoryBase<T>
    where T : class, IAggregateRoot;
