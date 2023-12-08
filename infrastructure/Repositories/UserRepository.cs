using infrastructure.DatabaseManager;
using infrastructure.DatabaseManager.Interface;
using infrastructure.Entities;
using infrastructure.Repositories.Interface;
using Npgsql;

namespace infrastructure.Repositories
{
    public class UserRepository : ICrud<Users>
    {
        private readonly IDBConnection _dbConnection;
        
        public UserRepository(IDBConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        
        public void Create(Users user)
        {
            using (var con = _dbConnection.GetConnection())
            {
                con.Open();

                const string sql = "INSERT INTO Users(email, admin) VALUES (@email, @admin)";

                using (var command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Admin", user.IsAdmin);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Users Read(int userId)
        {
            
            using (var con = _dbConnection.GetConnection())
            {
             con.Open();

             const string sql = "SELECT * FROM Users WHERE UserId = @UserId";

             using (var command = new NpgsqlCommand(sql,con))
             {
                 command.Parameters.AddWithValue("@UserId", userId);

                 using (var reader = command.ExecuteReader())
                 {
                     if (reader.Read())
                     {
                         return new Users
                         {
                             UserId = reader.GetInt32(reader.GetOrdinal("userid")),
                             Email = reader.GetString(reader.GetOrdinal("email")),
                             IsAdmin = reader.GetBoolean(reader.GetOrdinal("admin"))
                         };
                     }
                 }
             }
             
            }

            return null;
        }

        public void Update(Users user)
        {
            using (var con = _dbConnection.GetConnection())
            {
                con.Open();

                const string sql = "UPDATE Users SET email = @Email, admin = @Admin WHERE userId = @UserId";

                using (var command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@UserId", user.UserId);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Admin", user.IsAdmin);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int userId)
        {
            using (var con = _dbConnection.GetConnection())
            {
                con.Open();

                const string sql = "DELETE FROM Users WHERE userId = @UserId";

                using (var command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

