using MediatR;
using Mimas.Server.Application.Ports;

namespace Mimas.Server.Application.Features;

public class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, GetAllItemsQuery.Result>
{
    private readonly IItemRepository _itemRepository;

    public GetAllItemsQueryHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
    }

    public async Task<GetAllItemsQuery.Result> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await _itemRepository.GetAll();
        return new GetAllItemsQuery.Result(items);
    }
}