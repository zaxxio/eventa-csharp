using Microsoft.AspNetCore.Mvc;
using Web.Domain;
using Web.Query;

namespace Web.Controller;

[ApiController]
[Route("[controller]")]
public class ProductQueryController(IQueryDispatcher queryDispatcher)
{
    [HttpGet("/{productId}")]
    public Product FindById(Guid productId)
    {
        FindByProductId findByProductId = new FindByProductId();
        return queryDispatcher.Dispatch(findByProductId, ResponseType.InstanceOf<Product>());
    }
}