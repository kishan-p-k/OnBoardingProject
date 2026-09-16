using BT.Models;
using BT.Implementation.Providers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BT.Implementation.Providers.ADO;

public class CommentProvider : ICommentProvider
{
    private readonly DatabaseConnection _databaseConnection;
    private readonly ILogger<CommentProvider> _logger;

    public CommentProvider(
        DatabaseConnection databaseConnection,
        ILogger<CommentProvider> logger)
    {
        _databaseConnection = databaseConnection;
        _logger = logger;
    }

    public List<Comment> GetCommentByBug(string ref_id)
    {
        _logger.LogInformation(
            "Starting database operation to retrieve all Comments.");

        List<Comment> comments = new List<Comment>();

        try
        {
            using SqlConnection connection =
                _databaseConnection.CreateConnection();

            using SqlCommand command =
                new SqlCommand("GetCommentByBug", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@ref_id", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ref_id);

            connection.Open();

            _logger.LogDebug(
                "Database connection opened. Executing stored procedure {ProcedureName}.",
                "GetCommentByBug");

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Comment comment = new Comment
                {
                    reference_id = reader["reference_id"].ToString()!,
                    comment = reader["comment"].ToString()!,

                    author = reader["author"].ToString()!,

                    date = Convert.ToDateTime(
                        reader["date"])
                };

                comments.Add(comment);
            }

            _logger.LogInformation(
                "Successfully retrieved {CommnetCount} comments from the database.",
                comments.Count);

            return comments;
        }
        catch (SqlException ex)
        {
            _logger.LogError(
                ex,
                "Database error while retrieving comments.");

            throw;
        }
    }
}