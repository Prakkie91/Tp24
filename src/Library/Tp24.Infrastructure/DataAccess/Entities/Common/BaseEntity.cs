using System.ComponentModel.DataAnnotations;

namespace Tp24.Infrastructure.DataAccess.Entities.Common;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    [Key] public Guid Id { get; set; }
}