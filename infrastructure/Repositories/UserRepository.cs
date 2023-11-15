using infrastructure.Entities;
using Npgsql;

namespace infrastructure.Repositories
{
    public class UserRepository : ICrud<Users>
    {
        private readonly DBConnection _dbConnection;
        
        public UserRepository(DBConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        
        public void Create(Users user)
        {
            using (var con = DBConnection.GetConnection())
            {
                con.Open();

                const string sql = "INSERT INTO Users(email, admin) VALUES (@email, @Admin)";

                using (var command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Admin", user.IsAdmin);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Users Read(int Userid)
        {
            
            using (var con = DBConnection.GetConnection())
            {
             con.Open();

             const string sql = "SELECT * FROM Users WHERE UserId = @UserId";

             using (var command = new NpgsqlCommand(sql,con))
             {
                 command.Parameters.AddWithValue("@UserId", Userid);

                 using (var reader = command.ExecuteReader())
                 {
                     if (reader.Read())
                     {
                         return new Users
                         {
                             UserId = reader.GetInt32(reader.GetOrdinal("userId")),
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
            using (var con = DBConnection.GetConnection())
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

        public void Delete(int id)
        {
            using (var con = DBConnection.GetConnection())
            {
                con.Open();

                const string sql = "DELETE FROM Users WHERE userId = @UserId";

                using (var command = new NpgsqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@UserId", id);
                    command.ExecuteNonQuery();
                }
            }
        }
        
    }
}

