namespace LoanProcessing.Domain.Entities;
public abstract class Entity<TId>
{
    public TId Id { get; protected set; } = default!;
    public DateTime CreatedDate { get; protected set; } = DateTime.UtcNow;
    public bool IsActive { get; protected set; } = true;
    public bool IsDeleted { get; protected set; } = false;

    public void MarkAsDeleted()
    {
        IsDeleted = true;
        IsActive = false;
    }

    public void Reactivate()
    {
        IsDeleted = false;
        IsActive = true;
    }
}

