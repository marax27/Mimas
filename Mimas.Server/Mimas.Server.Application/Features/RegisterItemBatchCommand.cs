using MediatR;
using Mimas.Server.Application.Ports;

namespace Mimas.Server.Application.Features;

public class RegisterItemBatchCommand : IRequest
{
    public IReadOnlyCollection<string> ItemNames { get; }
    public string OwnerName { get; }

    public RegisterItemBatchCommand(IReadOnlyCollection<string>? itemNames, string ownerName)
    {
        ItemNames = itemNames ?? throw new ArgumentNullException(nameof(itemNames));
        if (itemNames.Count == 0)
            throw new ArgumentException("Empty item batch", nameof(itemNames));
        
        if (string.IsNullOrWhiteSpace(ownerName))
            throw new ArgumentException("Empty owner name.", nameof(ownerName));
        OwnerName = ownerName;
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
            .Select(itemName => new NewItemModel(itemName, 1, request.OwnerName, null))
            .ToArray();

        await _itemRepository.AddMany(items);
        return Unit.Value;
    }
}
