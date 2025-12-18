using CommsManager.Core.Entities;
using System.Linq.Expressions;

namespace CommsManager.Core.Specifications;

public sealed class ActiveOrdersSpecification : BaseSpecification<Order>
{
    public ActiveOrdersSpecification(Guid artistId)
        : base(o => o.ArtistProfileId == artistId && o.IsActive)
    {
        AddInclude(o => o.Customer);
        AddInclude(o => o.Attachments);
        ApplyOrderByDescending(o => o.CreatedDate);
    }

    public ActiveOrdersSpecification(Guid artistId, OrderStatus status)
        : base(o => o.ArtistProfileId == artistId && o.IsActive && o.Status == status)
    {
        AddInclude(o => o.Customer);
        AddInclude(o => o.Attachments);
        ApplyOrderByDescending(o => o.CreatedDate);
    }
}