using MediatR;
using Mimas.Server.Application.Ports;

namespace Mimas.Server.Application.Features;

public class RegisterItemBatchCommand : IRequest
{
    public IReadOnlyCollection<string> ItemNames { get; }
    public string OwnerName { get; }
    public string? AssignedBoxShortId { get; }

    public RegisterItemBatchCommand(IReadOnlyCollection<string>? itemNames, string ownerName, string? assignedBoxShortId)
    {
        ItemNames = itemNames ?? throw new ArgumentNullException(nameof(itemNames));
        if (itemNames.Count == 0)
            throw new ArgumentException("Empty item batch", nameof(itemNames));
        
        if (string.IsNullOrWhiteSpace(ownerName))
            throw new ArgumentException("Empty owner name.", nameof(ownerName));
        OwnerName = ownerName;

        AssignedBoxShortId = assignedBoxShortId;  // null value is valid here: it means items haven't been assigned to a box yet.
    }
}

public class RegisterItemBatchCommandHandler : IRequestHandler<RegisterItemBatchCommand>
{
    private readonly IItemRepository _itemRepository;
    private readonly IBoxRepository _boxRepository;
    private readonly ITimeProvider _timeProvider;

    public RegisterItemBatchCommandHandler(IItemRepository itemRepository, IBoxRepository boxRepository, ITimeProvider timeProvider)
    {
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        _boxRepository = boxRepository ?? throw new ArgumentNullException(nameof(boxRepository));
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    public async Task<Unit> Handle(RegisterItemBatchCommand request, CancellationToken cancellationToken)
    {
        await CreateBoxIfDoesNotExist(request.AssignedBoxShortId, request.OwnerName);

        var items = request.ItemNames
            .Select(itemName => new NewItemModel(itemName, 1, request.OwnerName, request.AssignedBoxShortId))
            .ToArray();

        await _itemRepository.AddMany(items);
        return Unit.Value;
    }

    private async Task CreateBoxIfDoesNotExist(string? boxShortId, string ownerName)
    {
        if (boxShortId is null)
            return;

        var containsBox = await _boxRepository.Contains(boxShortId);
        if (containsBox)
        {
            var timeNow = _timeProvider.GetTimeNow();
            var newBox = new AddBoxModel(boxShortId, ownerName, timeNow);
            await _boxRepository.Add(newBox);
        }
    }
}
