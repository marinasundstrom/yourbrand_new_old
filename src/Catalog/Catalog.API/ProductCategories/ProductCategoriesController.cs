using Catalog.API.Data;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Catalog.API.ProductCategories;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductCategoriesController : Controller
{
    [HttpGet("{*idOrPath}")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ProductCategory>> GetProductCategoryById(string idOrPath, IMediator mediator, CatalogContext catalogContext, CancellationToken cancellationToken) 
    {
        var result = await mediator.Send(new GetProductCategoryById(idOrPath), cancellationToken);

        return result.IsSuccess ? Ok(result.GetValue()) : NotFound();
    }
}