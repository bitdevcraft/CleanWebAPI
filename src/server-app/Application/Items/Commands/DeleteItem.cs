using Application.Common.Core;
using Application.Common.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Items.Commands;

public class DeleteItem
{
    public class Command : IRequest<ErrorOr<Unit>>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, ErrorOr<Unit>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ErrorOr<Unit>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var item = await _context.Items.FindAsync(request.Id);

            if (item == null)
                return Error.NotFound();

            _context.Items.Remove(item);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
