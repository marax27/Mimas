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

    public async Task<bool> Contains(string boxShortId)
    {
        await using var connection = new SqlConnection(_settings.Value.ConnectionString);
        await connection.OpenAsync();
        var shortIds = await connection.QueryAsync<string>(FindShortId, new { ShortId = boxShortId });
        return boxShortId.Any();
    }

    public async Task Add(AddBoxModel box)
    {
        await using var connection = new SqlConnection(_settings.Value.ConnectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync(AddBox, box);
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

    private const string FindShortId = $@"
SELECT
    b.short_id
FROM
    Boxes b
WHERE
    b.short_id = @ShortId
";

    private const string AddBox = $@"
INSERT INTO Boxes
    (short_id, owner_id, registered_on)
VALUES
    (
        @{nameof(AddBoxModel.ShortId)},
        (SELECT o.id FROM Owners o WHERE o.name = @{nameof(AddBoxModel.OwnerName)}),
        @{nameof(AddBoxModel.RegisteredOn)}
    )
";
}
