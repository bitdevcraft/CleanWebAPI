using Application.Common.Core;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Items.Commands;

public class EditItem
{
    public class Command : IRequest<Result<Unit>>
    {
        public Item Item { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var item = await _context.Items.FindAsync(request.Item.Id, cancellationToken);

            if (item == null)
                return null;

            _mapper.Map(request.Item, item);

            await _context.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
