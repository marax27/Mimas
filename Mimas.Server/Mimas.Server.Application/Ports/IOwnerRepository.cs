namespace Mimas.Server.Application.Ports;

public record Owner(
    Guid Id,
    string Name
);

public interface IOwnerRepository
{
    public Task<IEnumerable<Owner>> GetAll();
}
