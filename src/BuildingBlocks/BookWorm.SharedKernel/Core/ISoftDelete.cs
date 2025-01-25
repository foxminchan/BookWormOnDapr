namespace BookWorm.SharedKernel.Core;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }

    void Delete();
}
