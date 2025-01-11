namespace BookWorm.SharedKernel.Models;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }

    void Delete();
}
