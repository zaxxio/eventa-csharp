using Core.Streotype;
using Web.Event;

[ProjectionGroup]
public class ProductProjection
{
    [EventHandler]
    public void On(ProductCreatedEvent productCreatedEvent)
    {
        // MYSQL
        // Postgre
    }

    [EventHandler]
    public void On(ProductUpdatedEvent productUpdatedEvent)
    {
        
    }

    [QueryHandler]
    public List<string> GetProductById()
    {
        return new List<string>();
    }
    
}