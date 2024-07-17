using Core.Streotype;
using Web.Event;
using Web.Query;

namespace Web.Projection;

[ProjectionGroup]
public class ProductProjection
{
    [EventHandler]
    public void On(ProductCreatedEvent productCreatedEvent)
    {
        
    }

    [EventHandler]
    public void On(ProductUpdatedEvent productUpdatedEvent)
    {
    }

    [EventHandler]
    public void On(ProductDeletedEvent productDeletedEvent)
    {
    }

    [QueryHandler]
    public List<string> GetProductById(FindByProductId findByProductId)
    {
        return new List<string>();
    }
}