namespace Tp24.Core.Domain.Entities.Common;

public abstract class BaseDomainModel
{
    protected BaseDomainModel()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
}