using System;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace AngelusTBlog.Data
{
    public class ConnectionService
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            // Startup code
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // look for the Environment Variable - if running locally
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            // look for the Environment Variable - if NOT running locally
            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }

        // Create the Uri
        private static string BuildConnectionString(string databaseUrl)
        {
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(":");

            return new NpgsqlConnectionStringBuilder()
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Prefer,
                TrustServerCertificate = true
            }.ToString();
        }
    }
}
