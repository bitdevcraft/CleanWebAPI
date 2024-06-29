using Application.Common.Core;
using Application.Common.Interfaces;
using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Items.Commands;

public class CreateItem
{
    public class Command : IRequest<ErrorOr<int>>
    {
        public Item Item { get; set; }
    }

    public class Handler : IRequestHandler<Command, ErrorOr<int>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ErrorOr<int>> Handle(Command request, CancellationToken cancellationToken)
        {
            await _context.Items.AddAsync(request.Item);
            await _context.SaveChangesAsync(cancellationToken);
            return request.Item.Id;
        }
    }
}
