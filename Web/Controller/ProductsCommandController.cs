using Core.Dispatcher;
using Microsoft.AspNetCore.Mvc;
using Web.Command;

namespace Web.Controller;

[ApiController]
[Route("[controller]")]
public class ProductsCommandController(ICommandDispatcher commandDispatcher) : ControllerBase
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