using Application.Common.Core;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Items.Queries;

public class ListItems
{
    public class Query : IRequest<Result<List<Item>>> { }

    public class Handler : IRequestHandler<Query, Result<List<Item>>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<Item>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var result = await _context.Items.ToListAsync(cancellationToken);

            return Result<List<Item>>.Success(result);
        }
    }
}
