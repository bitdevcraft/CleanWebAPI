using Domain.Common;

namespace Domain.Entities;

public class Item : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
}
