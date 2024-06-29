using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Items.Queries;

public class GetItem
{
    public class Query : IRequest<Item>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Item>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Item> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Items.FindAsync(request.Id, cancellationToken);
        }
    }
}
