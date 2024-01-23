using Dapper;
using infrastructure.DatabaseManager.Interface;
using infrastructure.Entities;
using infrastructure.Repositories.Interface;
using Npgsql;

namespace infrastructure.Repositories
{
    public class UserRepository : ICrud<Users>
    {
        private readonly IDBConnection _dbConnection;
        private string schema = "";
        
        public UserRepository(IDBConnection dbConnection)
        {
            _dbConnection = dbConnection;
            if (Environment.GetEnvironmentVariable("IsTestMode")=="true")
            {
                schema = "tests.";
            }
            else
            {
                schema = "public.";
            }
        }
        
        public Users Create(Users user)
        {
            var sql = $@"INSERT INTO "+ schema + "Users(email, isadmin) VALUES (@email, @isadmin) RETURNING *";

            using (var con = _dbConnection.GetConnection())
            {
                con.Open();

                return con.QueryFirst<Users>(sql, new { email = user.Email, isadmin = user.IsAdmin});
            }
        }

        public Users Read(int userId)
        {
            var sql = $@"SELECT * FROM "+ schema +"Users WHERE UserId = @UserId";
            using (var con = _dbConnection.GetConnection())
            {
                con.Open();
                return con.QueryFirst<Users>(sql, new { UserId = userId});
            }
        }

        public void Update(Users user)
        {
            var sql = $@"UPDATE " + schema + "Users SET email = @Email, isadmin = @IsAdmin WHERE userId = @UserId";

            using (var con = _dbConnection.GetConnection())
            {
                con.Open();
                con.Execute(sql, new { Email = user.Email, IsAdmin = user.IsAdmin, UserId = user.UserId });
            }
        }

        public void Delete(int userId)
        {
            var sql = $@"DELETE FROM "+ schema +"Users WHERE userid = @UserId";
            
            using (var con = _dbConnection.GetConnection())
            {
                con.Open();
                con.Execute(sql, new { UserId = userId });
            }
        }
        
    }
}

