using Application.Common.Core;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Items.Commands;

public class DeleteItem
{
    public class Command : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var item = await _context.Items.FindAsync(request.Id);

            if (item == null)
                return null;

            _context.Items.Remove(item);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
