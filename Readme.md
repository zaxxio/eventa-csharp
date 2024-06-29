## Aggregate
```csharp
[Aggregate]
public class ProductAggregate : AggregateRoot
{
    [RoutingKey] 
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }

    public ProductAggregate()
    {
    }

    [CommandHandler]
    public ProductAggregate(CreateProductCommand createProductCommand)
    {
        Debug.WriteLine($"Invoked Command Handler {createProductCommand}.");
        Apply(
            new ProductCreatedEvent()
            {
                Id = createProductCommand.Id,
                Title = createProductCommand.Title,
                Quantity = createProductCommand.Quantity,
                Price = createProductCommand.Price
            }
        );
    }

    [EventSourcingHandler]
    public void On(ProductCreatedEvent productCreatedEvent)
    {
        Id = productCreatedEvent.Id;
        Title = productCreatedEvent.Title;
        Price = productCreatedEvent.Price;
        Quantity = productCreatedEvent.Quantity;
        Debug.WriteLine($"Create Invoked Event Sourcing Handler {productCreatedEvent}.");
    }

    [CommandHandler]
    public void Handle(UpdateProductCommand updateProductCommand)
    {
        Debug.WriteLine($"Invoked Command Handler {updateProductCommand}.");
        Apply(
            new ProductUpdatedEvent()
            {
                Id = updateProductCommand.Id,
                Title = updateProductCommand.Title,
                Quantity = updateProductCommand.Quantity,
                Price = updateProductCommand.Price
            });
    }

    [EventSourcingHandler]
    public void On(ProductUpdatedEvent productUpdatedEvent)
    {
        Id = productUpdatedEvent.Id;
        Title = productUpdatedEvent.Title;
        Price = productUpdatedEvent.Price;
        Quantity = productUpdatedEvent.Quantity;
        Debug.WriteLine($"Update Invoked Event Sourcing Handler {productUpdatedEvent}.");
    }

    [CommandHandler]
    public void Handle(DeleteProductCommand deleteProductCommand)
    {
        Debug.WriteLine($"Invoked Command Handler {deleteProductCommand}.");
        Apply(
            new ProductDeletedEvent()
            {
                Id = deleteProductCommand.Id,
                Title = deleteProductCommand.Title,
                Quantity = deleteProductCommand.Quantity,
                Price = deleteProductCommand.Price
            }
        );
    }

    [EventSourcingHandler]
    public void On(ProductDeletedEvent productDeletedEvent)
    {
        Id = productDeletedEvent.Id;
        Title = productDeletedEvent.Title;
        Price = productDeletedEvent.Price;
        Quantity = productDeletedEvent.Quantity;
        Debug.WriteLine($"Delete Invoked Event Sourcing Handler {productDeletedEvent}.");
    }
}
```
## Command Dispatcher 
```csharp

[ApiController]
[Route("[controller]")]
public class ProductsController(ICommandDispatcher commandDispatcher) : ControllerBase
{
    [HttpPost]
    [Route("CreateProduct")]
    public IActionResult CreateProduct([FromBody] CreateProductCommand createProductCommand)
    {
        commandDispatcher.Dispatch(createProductCommand);
        return Ok($"Product creation request accepted.");
    }

    [HttpPut]
    [Route("UpadateProduct")]
    public IActionResult UpdateProduct([FromBody] UpdateProductCommand updateProductCommand)
    {
        commandDispatcher.Dispatch(updateProductCommand);
        return Ok($"Product updated request accepted.");
    }

    [HttpDelete]
    [Route("DeleteProduct")]
    public IActionResult DeleteProduct([FromBody] DeleteProductCommand deleteProductCommand)
    {
        commandDispatcher.Dispatch(deleteProductCommand);
        return Ok($"Product delete request accepted.");
    }
}
```
## Infrastructure Dependency
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Kafka": {
    "BootstrapServers": "localhost:9092",
    "Topic": "BaseEvent"
  },
  "MongoDbConfig": {
    "ConnectionString": "mongodb://localhost:27017",
    "AuthenticationDatabase": "admin",
    "Database": "EventStore",
    "Collection": "Events",
    "Username": "username",
    "Password": "password"
  }
}
```