
using MediatR;
using Mimas.Server.Application.Ports;

namespace Mimas.Server.Application.Features;

public class GetAllBoxesQuery : IRequest<GetAllBoxesQuery.Result>
{
    public record Result(IEnumerable<BoxViewModel> Boxes);
}

public class GetAllBoxesQueryHandler : IRequestHandler<GetAllBoxesQuery, GetAllBoxesQuery.Result>
{
    private readonly IBoxRepository _boxRepository;

    public GetAllBoxesQueryHandler(IBoxRepository boxRepository)
    {
        _boxRepository = boxRepository ?? throw new ArgumentNullException(nameof(boxRepository));
    }

    public async Task<GetAllBoxesQuery.Result> Handle(GetAllBoxesQuery request, CancellationToken cancellationToken)
    {
        var boxes = await _boxRepository.GetAll();
        return new(boxes);
    }
}
