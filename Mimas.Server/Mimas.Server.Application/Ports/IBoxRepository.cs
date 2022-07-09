namespace Mimas.Server.Application.Ports;

public record BoxViewModel(
    string ShortId,
    string OwnerName,
    int ItemCount,
    DateTime RegisteredOn,
    DateTime? DeliveredOn
);

public record AddBoxModel(
    string ShortId,
    string OwnerName,
    DateTime RegisteredOn
);

public interface IBoxRepository
{
    Task<IEnumerable<BoxViewModel>> GetAll();
    Task<bool> Contains(string boxShortId);
    Task Add(AddBoxModel box);
    Task MarkAsDelivered(string shortId, DateTime deliveredOn);
}
