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

    public RegisterItemBatchCommandHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
    }

    public async Task<Unit> Handle(RegisterItemBatchCommand request, CancellationToken cancellationToken)
    {
        var items = request.ItemNames
            .Select(itemName => new NewItemModel(itemName, 1, request.OwnerName, request.AssignedBoxShortId))
            .ToArray();

        await _itemRepository.AddMany(items);
        return Unit.Value;
    }
}
