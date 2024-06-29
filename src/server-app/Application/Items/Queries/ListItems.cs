using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Items.Queries;

public class ListItems
{
    public class Query : IRequest<List<Item>> { }

    public class Handler : IRequestHandler<Query, List<Item>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Items.ToListAsync(cancellationToken);
        }
    }
}
