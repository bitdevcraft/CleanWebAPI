using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Items.Commands;

public class EditItem
{
    public class Command : IRequest<Unit>
    {
        public Item Item { get; set; }
    }

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var item = await _context.Items.FindAsync(request.Item.Id, cancellationToken);

            _mapper.Map(request.Item, item);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
