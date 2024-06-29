using Core.Command;
using Core.Streotype;

namespace Web.Command;

public class DeleteProductCommand : BaseCommand
{
    [RoutingKey] public Guid Id { get; set; }
    public string? Title { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
}