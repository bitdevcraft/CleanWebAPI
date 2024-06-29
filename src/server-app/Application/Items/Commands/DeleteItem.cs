using Application.Common.Interfaces;
using MediatR;

namespace Application.Items.Commands;

public class DeleteItem
{
    public class Command : IRequest<Unit>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var item = await _context.Items.FindAsync(request.Id);

            _context.Items.Remove(item);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
