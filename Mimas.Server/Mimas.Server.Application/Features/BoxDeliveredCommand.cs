using MediatR;
using Mimas.Server.Application.Ports;

namespace Mimas.Server.Application.Features;

public class BoxDeliveredCommand : IRequest
{
    public string BoxShortId { get; }

    public BoxDeliveredCommand(string boxShortId)
    {
        if (string.IsNullOrWhiteSpace(boxShortId))
            throw new ArgumentNullException(nameof(boxShortId));
        BoxShortId = boxShortId;
    }
}

public class BoxDeliveredCommandHandler : IRequestHandler<BoxDeliveredCommand>
{
    private readonly IBoxRepository _boxRepository;
    private readonly ITimeProvider _timeProvider;

    public BoxDeliveredCommandHandler(IBoxRepository boxRepository, ITimeProvider timeProvider)
    {
        _boxRepository = boxRepository ?? throw new ArgumentNullException(nameof(boxRepository));
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    public async Task<Unit> Handle(BoxDeliveredCommand request, CancellationToken cancellationToken)
    {
        var timeNow = _timeProvider.GetTimeNow();
        await _boxRepository.MarkAsDelivered(request.BoxShortId, timeNow);

        return Unit.Value;
    }
}