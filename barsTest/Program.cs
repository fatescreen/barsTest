using Npgsql;
using System;

namespace barsTest
{
    class Program
    {
        static void Main(string[] args)
        {

            var cs = "Host=localhost;Username=postgres;Password=lol123;Database=postgres";

            using var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = "SELECT * from test";

            using var cmd = new NpgsqlCommand(sql, con);

            var version = cmd.ExecuteScalar().ToString();
            Console.WriteLine($"PostgreSQL version: {version}");
        }
    }
}
