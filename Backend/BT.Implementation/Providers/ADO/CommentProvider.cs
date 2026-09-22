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
    public Comment UpdateComment(string reference_id, string updatedComment)
    {
        _logger.LogInformation("Starting database operation to update comment with reference ID: {ReferenceId}", reference_id);
        try
        {
            using SqlConnection connection = _databaseConnection.CreateConnection();
            using SqlCommand command = new SqlCommand("UpdateComment", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@reference_id", SqlDbType.UniqueIdentifier).Value = Guid.Parse(reference_id);
            command.Parameters.Add("@comment", SqlDbType.VarChar, -1).Value = updatedComment;

            SqlParameter updatedDateParam = command.Parameters.Add("@updated_date", SqlDbType.DateTime2);
            updatedDateParam.Direction = ParameterDirection.Output;

            connection.Open();
            _logger.LogDebug("Database connection opened. Executing stored procedure {ProcedureName}.", "UpdateComment");
            Console.WriteLine($"Executing stored procedure UpdateComment with reference ID: {reference_id} and updated comment: {updatedComment}");

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected != 0)
            {
                _logger.LogInformation("Successfully updated comment with reference ID: {ReferenceId}", reference_id);
                Console.WriteLine($"Successfully updated comment with reference ID: {reference_id}");

                DateTime updatedDate = updatedDateParam.Value != DBNull.Value 
                    ? (DateTime)updatedDateParam.Value 
                    : DateTime.UtcNow;

                return new Comment
                {
                    reference_id = reference_id,
                    comment = updatedComment,
                    author = "",
                    date = updatedDate
                };
            }
            else
            {
                _logger.LogWarning("No comment found with reference ID: {ReferenceId} to update.", reference_id);
                Console.WriteLine($"No comment found with reference ID: {reference_id} to update.");
                throw new Exception($"No comment found with reference ID: {reference_id} to update.");
            }
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Database error while updating comment with reference ID: {ReferenceId}", reference_id);
            Console.WriteLine($"Database error while updating comment with reference ID: {reference_id}: {ex.Message}");
            throw;
        }
    }

    public Comment CreateComment(string ref_id, string comment, string author)
    {
        _logger.LogInformation(
            "Starting database operation to create new Comment.");

        try
        {
            using SqlConnection connection =
                _databaseConnection.CreateConnection();

            using SqlCommand command =
                new SqlCommand("CreateComment", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.Add("@ReferenceId", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ref_id);
            command.Parameters.Add("@Comment", SqlDbType.VarChar, 255).Value = comment;
            command.Parameters.Add("@Author", SqlDbType.VarChar, 100).Value = author;


            connection.Open();

            _logger.LogDebug(
                "Database connection opened. Executing stored procedure {ProcedureName}.",
                "GetCommentByBug");

            using SqlDataReader reader = command.ExecuteReader();

            if (!reader.Read())
            {
                _logger.LogWarning(
                    "Stored procedure returned no rows");
                return null;
            }

            _logger.LogInformation(
                "Stored procedure returned a bug row. Reading result.");

            var newcomment = new Comment
            {
                reference_id = reader["reference_id"].ToString()!,

                comment = reader["comment"].ToString()!,

                author = reader["author"].ToString()!,

                date = Convert.ToDateTime(
                    reader["date"])
            };

            _logger.LogInformation(
                "Comment successfully read after creation.");

            return newcomment;
        }
        catch (FormatException ex)
        {
            _logger.LogError(
                ex,
                "Invalid");

            throw;
        }
        catch (SqlException ex)
        {
            _logger.LogError(
                ex,
                "Database error while executing CreateComment+");

            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while creating comment.");

            throw;
        }
    }
}