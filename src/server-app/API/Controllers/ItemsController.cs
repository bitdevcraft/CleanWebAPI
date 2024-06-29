using API.Controllers.Base;
using Application.Items.Commands;
using Application.Items.Queries;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ItemsController : ApiController
{
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await Mediator.Send(new ListItems.Query());
        return result.Match(items => Ok(items), Problem);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await Mediator.Send(new GetItem.Query { Id = id });
        return result.Match(item => Ok(item), Problem);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Item item)
    {
        var result = await Mediator.Send(new CreateItem.Command { Item = item });
        return result.Match(id => Ok(id), Problem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Item item)
    {
        item.Id = id;
        var result = await Mediator.Send(new EditItem.Command { Item = item });
        return result.Match(_ => NoContent(), Problem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await Mediator.Send(new DeleteItem.Command { Id = id });
        return result.Match(_ => NoContent(), Problem);
    }
}
