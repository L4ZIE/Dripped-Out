using Dapper;
using infrastructure.DatabaseManager.Interface;
using Npgsql;

namespace infrastructure.DatabaseManager
{
    public class DBConnection : IDBConnection
    {
        public static readonly Uri Uri;
        public static readonly string ProperlyFormattedConnectionString;


        static DBConnection()
        {
            const string envVarKeyName = "dbconnectionstring";

            var rawConnectionString = Environment.GetEnvironmentVariable(envVarKeyName);
            if (rawConnectionString == null)
            {
                throw new Exception("Environment variable for the database connection string is not set!");
            }

            try
            {
                Uri = new Uri(rawConnectionString);
                ProperlyFormattedConnectionString = string.Format(
                    "Host={0};Database={1};Username={2};Password={3};",
                    Uri.Host,
                    Uri.AbsolutePath.Trim('/'),
                    Uri.UserInfo.Split(':')[0],
                    Uri.UserInfo.Split(':')[1]);
            }
            catch
            {
                throw new Exception("Connection string found but could not be formatted correctly!");
            }
        }
        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ProperlyFormattedConnectionString);
        }

        public void TriggerRebuild()
        {
            var sql = "DROP SCHEMA IF EXISTS tests CASCADE; " +
                      "CREATE SCHEMA tests; " +
                      "CREATE TABLE tests.users (" +
                      "userid serial PRIMARY KEY, " +
                      "email text, " +
                      "isadmin boolean); " +
                      "CREATE TABLE tests.colortype (" +
                      "colorid serial Primary Key, " +
                      "color text); " +
                      "INSERT INTO tests.users(email, isadmin) VALUES ('test1@dapper.com', false); " +
                      "INSERT INTO tests.users(email, isadmin) VALUES ('test2@dapper.com', false); " +
                      "INSERT INTO tests.users(email, isadmin) VALUES ('test3@dapper.com', true); " +
                      "INSERT INTO tests.colortype(color) VALUES ('Black'); " +
                      "INSERT INTO tests.colortype(color) VALUES ('White'); " +
                      "INSERT INTO tests.colortype(color) VALUES ('Blue'); ";

            using (var conn = this.GetConnection())
            {
                conn.Open();
                conn.Execute(sql);
            }
        }
    }
}