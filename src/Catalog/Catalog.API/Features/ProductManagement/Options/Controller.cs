using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Features.ProductManagement.Options;

[ApiController]
//[ApiVersion("1")]
[Route("api/options")] //v{version:apiVersion}/
public class OptionsController : Controller
{
    private readonly IMediator _mediator;

    public OptionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<OptionDto>> GetOptions(bool includeChoices = false)
    {
        return Ok(await _mediator.Send(new GetOptions(includeChoices)));
    }

    /*
    [HttpGet("{optionId}")]
    public async Task<ActionResult<OptionDto>> GetProductOptionValues(string optionId)
    {
        return Ok(await _mediator.Send(new GetOption(optionId)));
    }
    */

    [HttpGet("{id}/values")]
    public async Task<ActionResult<OptionValueDto>> GetOptionValues(string id)
    {
        return Ok(await _mediator.Send(new GetOptionValues(id)));
    }
}