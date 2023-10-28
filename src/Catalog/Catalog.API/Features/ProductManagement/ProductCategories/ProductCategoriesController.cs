using Catalog.API.Persistence;

using MediatR;

using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Features.ProductManagement.ProductCategories;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/productCategories")]
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