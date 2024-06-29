using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Items.Commands;

public class CreateItem
{
    public class Command : IRequest<int>
    {
        public Item Item { get; set; }
    }

    public class Handler : IRequestHandler<Command, int>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            await _context.Items.AddAsync(request.Item);
            await _context.SaveChangesAsync(cancellationToken);
            return request.Item.Id;
        }
    }
}
