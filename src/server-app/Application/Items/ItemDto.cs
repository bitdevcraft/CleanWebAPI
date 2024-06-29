using AutoMapper;
using Domain.Entities;

namespace Application.Items;

public class ItemDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class Mapping : Profile
{
    public Mapping()
    {
        CreateMap<Item, Item>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Item, ItemDto>();
    }
}
