using Application.Common.Core;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Items.Queries;

public class GetItem
{
    public class Query : IRequest<Result<Item>>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<Item>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Item>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _context.Items.FindAsync(request.Id, cancellationToken);

            if (result == null)
                return null;

            return Result<Item>.Success(result);
        }
    }
}
