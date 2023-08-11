using Catalog.API.Data;
using Catalog.API.Model;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Catalog.API.ProductCategories;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductCategoriesController : Controller
{
    [HttpGet("{*idOrPath}")]
    [ProducesDefaultResponseType]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ProductCategory>> GetProductCategoryById(string idOrPath, CatalogContext catalogContext, CancellationToken cancellationToken) 
    {
        var isId = int.TryParse(idOrPath, out var id);

        ProductCategory? productCategory;

        if (isId)
        {
            productCategory = await catalogContext.ProductCategories.FirstOrDefaultAsync(productCategory => productCategory.Id == id, cancellationToken);
        }
        else
        {
            idOrPath = WebUtility.UrlDecode(idOrPath);

            productCategory = await catalogContext.ProductCategories.FirstOrDefaultAsync(productCategory => productCategory.Path == idOrPath, cancellationToken);
        }

        return productCategory is not null ? Ok(productCategory) : NotFound();
    }
}