using Mimas.Server.Application.Ports;

namespace Mimas.Server.Infrastructure
{
    public class StandardTimeProvider : ITimeProvider
    {
        public DateTime GetTimeNow()
        {
            return DateTime.Now;
        }
    }
}
