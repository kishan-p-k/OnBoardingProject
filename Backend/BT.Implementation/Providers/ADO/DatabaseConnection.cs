using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace BT.Implementation.Providers.ADO;

public class DatabaseConnection
{
    private readonly IConfiguration _configuration;
    public DatabaseConnection(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public SqlConnection CreateConnection()
    {
        var connectionString =
        _configuration.GetConnectionString("DefaultConnection");
        return new SqlConnection(connectionString);
    }
    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            using var connection = CreateConnection();
            await connection.OpenAsync();
            return connection.State == System.Data.ConnectionState.Open;
        }
        catch
        {
            return false;
        }
    }
}