namespace Temp.Domain.Entities.Base;

public abstract class AuditableEntity
{
    public Guid? CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? ModifiedById { get; set; }

    public DateTime ModifiedAt { get; set; }

    public bool IsDeleted { get; set; } = false;
}
