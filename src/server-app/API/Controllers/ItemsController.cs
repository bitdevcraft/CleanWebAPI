using Application.Items.Commands;
using Application.Items.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private IMediator _mediator;
    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await Mediator.Send(new ListItems.Query());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await Mediator.Send(new GetItem.Query { Id = id });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Item item)
    {
        var result = await Mediator.Send(new CreateItem.Command { Item = item });
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Item item)
    {
        item.Id = id;
        await Mediator.Send(new EditItem.Command { Item = item });
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteItem.Command { Id = id });
        return Ok();
    }
}
