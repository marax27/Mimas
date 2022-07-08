namespace Mimas.Server.Application.Ports;

public record BoxViewModel(
    string ShortId,
    string OwnerName,
    int ItemCount,
    DateTime RegisteredOn,
    DateTime? DeliveredOn
);

public interface IBoxRepository
{
    Task<IEnumerable<BoxViewModel>> GetAll();
}
