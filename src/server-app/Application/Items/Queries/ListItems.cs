using Application.Common.Core;
using Application.Common.Interfaces;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Items.Queries;

public class ListItems
{
    public class Query : IRequest<ErrorOr<List<Item>>> { }

    public class Handler : IRequestHandler<Query, ErrorOr<List<Item>>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ErrorOr<List<Item>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            return await _context.Items.ToListAsync(cancellationToken);
        }
    }
}
