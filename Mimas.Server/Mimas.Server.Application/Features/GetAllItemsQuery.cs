using MediatR;
using Mimas.Server.Application.Ports;

namespace Mimas.Server.Application.Features;

public class GetAllItemsQuery : IRequest<GetAllItemsQuery.Result>
{
    public record Result(IEnumerable<ItemViewModel> Items);
}