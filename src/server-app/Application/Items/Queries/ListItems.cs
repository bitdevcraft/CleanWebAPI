using Application.Common.Core;
using Application.Common.Interfaces;
using Application.Common.Security.Policies;
using Application.Common.Security.Request;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Items.Queries;

public class ListItems
{
    [Authorize(Policies = Policy.SystemAdministrator)]
    public class Query : IAuthorizeableRequest<ErrorOr<List<Item>>> { }

    public class Handler : IRequestHandler<Query, ErrorOr<List<Item>>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ErrorOr<List<Item>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            return await _context.Items.ToListAsync(cancellationToken);
        }
    }
}
