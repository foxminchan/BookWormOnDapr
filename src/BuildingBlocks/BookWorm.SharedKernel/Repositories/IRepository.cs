using Ardalis.Specification;
using BookWorm.SharedKernel.Core.Model;

namespace BookWorm.SharedKernel.Repositories;

public interface IRepository<T> : IRepositoryBase<T>
    where T : class, IAggregateRoot;
