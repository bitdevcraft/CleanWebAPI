using API.Controllers.Base;
using Application.Items.Commands;
using Application.Items.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ItemsController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await Mediator.Send(new ListItems.Query());
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await Mediator.Send(new GetItem.Query { Id = id });
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Item item)
    {
        var result = await Mediator.Send(new CreateItem.Command { Item = item });
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Item item)
    {
        item.Id = id;
        var result = await Mediator.Send(new EditItem.Command { Item = item });
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await Mediator.Send(new DeleteItem.Command { Id = id });
        return HandleResult(result);
    }
}
