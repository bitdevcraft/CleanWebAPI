using Application.Common.Core;
using Application.Common.Interfaces;
using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Items.Queries;

public class GetItem
{
    public class Query : IRequest<ErrorOr<Item>>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, ErrorOr<Item>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ErrorOr<Item>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _context.Items.FindAsync(request.Id, cancellationToken);

            if (result == null)
                return Error.NotFound();

            return result;
        }
    }
}
