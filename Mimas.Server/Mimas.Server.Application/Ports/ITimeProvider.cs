namespace Mimas.Server.Application.Ports;

public interface ITimeProvider
{
    DateTime GetTimeNow();
}
