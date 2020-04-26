using System;


public class PostgreSQL : IPostgreSQL
{
	private string connectionString;
	
	public PostgreSQL(string connectionString)
	{
		this.connectionString = connectionString;
	}


	public void testFunc()
	{
		using var con = new NpgsqlConnection(cs);

		con.Open();

		var sql = "SELECT * from test";

		using var cmd = new NpgsqlCommand(sql, con);

		var version = cmd.ExecuteScalar().ToString();
		Console.WriteLine($"PostgreSQL version: {version}");
	}
}

