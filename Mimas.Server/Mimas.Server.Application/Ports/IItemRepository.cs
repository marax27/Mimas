namespace Mimas.Server.Application.Ports;

public record ItemViewModel(
    int Id,
    string Name,
    int Count,
    string OwnerName,
    string? AssignedBoxShortId
);

public record NewItemModel(
    string Name,
    int ItemCount,
    string OwnerName,
    string? AssignedBoxShortId
);

public interface IItemRepository
{
    public Task<IEnumerable<ItemViewModel>> GetAll();
    public Task AddMany(IEnumerable<NewItemModel> newItem);
}
