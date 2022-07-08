using Dapper;
using Microsoft.Extensions.Options;
using Mimas.Server.Application.Ports;
using System.Data.SqlClient;

namespace Mimas.Server.Infrastructure.Repositories;

public class DbBoxRepository : IBoxRepository
{
    private readonly IOptions<DatabaseConnectionSettings> _settings;

    public DbBoxRepository(IOptions<DatabaseConnectionSettings> settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    public async Task<IEnumerable<BoxViewModel>> GetAll()
    {
        await using var connection = new SqlConnection(_settings.Value.ConnectionString);
        await connection.OpenAsync();
        var result = await connection.QueryAsync<BoxViewModel>(SelectAllBoxes);
        return result;
    }

    private const string SelectAllBoxes = $@"
SELECT
    b.short_id      AS {nameof(BoxViewModel.ShortId)},
    o.name          AS {nameof(BoxViewModel.OwnerName)},
    (SELECT COUNT(*) FROM Items j WHERE j.box_id = b.id) AS {nameof(BoxViewModel.ItemCount)},
    b.registered_on AS {nameof(BoxViewModel.RegisteredOn)},
    b.delivered_on  AS {nameof(BoxViewModel.DeliveredOn)}
FROM Boxes b
INNER JOIN Owners o ON o.id = b.owner_id
";
}
