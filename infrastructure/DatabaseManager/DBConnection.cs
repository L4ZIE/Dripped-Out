using System;
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
    }
}