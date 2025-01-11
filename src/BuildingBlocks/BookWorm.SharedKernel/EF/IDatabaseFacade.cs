using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BookWorm.SharedKernel.EF;

public interface IDatabaseFacade
{
    DatabaseFacade Database { get; }
}
