using Application.Common.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BaseApiController : ControllerBase
{
    private IMediator _mediator;

    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result == null)
            return NotFound();

        if (result.IsSuccess && result.Value != null)
            return Ok(result.Value);

        if (result.IsSuccess && result.Value == null)
            return NotFound();

        return BadRequest(result.Error);
    }
}
