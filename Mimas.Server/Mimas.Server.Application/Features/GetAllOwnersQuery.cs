using MediatR;
using Mimas.Server.Application.Ports;

namespace Mimas.Server.Application.Features;

public class GetAllOwnersQuery : IRequest<GetAllOwnersQuery.Result>
{
    public record Result(IEnumerable<Owner> Owners);
}

public class GetAllOwnersQueryHandler : IRequestHandler<GetAllOwnersQuery, GetAllOwnersQuery.Result>
{
    private readonly IOwnerRepository _ownerRepository;

    public GetAllOwnersQueryHandler(IOwnerRepository ownerRepository)
    {
        _ownerRepository = ownerRepository ?? throw new ArgumentNullException(nameof(ownerRepository));
    }

    public async Task<GetAllOwnersQuery.Result> Handle(GetAllOwnersQuery request, CancellationToken cancellationToken)
    {
        var owners = await _ownerRepository.GetAll();
        return new(owners);
    }
}
