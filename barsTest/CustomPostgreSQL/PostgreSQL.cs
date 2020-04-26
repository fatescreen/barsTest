using System;
using Npgsql;

namespace barsTest.CustomPostgreSQL
{

	public class PostgreSQL : IPostgreSQL
	{
		private string connectionString;
		private NpgsqlConnection connection;
		private string dataBaseName;

		public PostgreSQL(string connectionString)
		{
			this.connectionString = connectionString;
			this.connection = new NpgsqlConnection(this.connectionString);
			this.dataBaseName = connectionString.Substring(connectionString.IndexOf("Database=")).Replace("Database=", "");
		}

		public float getDataBaseSizeInGb()
		{
			float result = 0;

			string[] sizeInTwoWords = getDataBaseSize().Split(' ');
			string size = sizeInTwoWords[0];
			string bytePower = sizeInTwoWords[1];			
			
			if (float.Parse(size) < 0)
			{
				return -1;
			}

			if (bytePower == "B")
			{
				result = float.Parse(size) / 1024 / 1024 / 1024;
			}
			else if (bytePower == "kB")
			{
				result = float.Parse(size) / 1024 / 1024;
			} 
			else if (bytePower == "MB")
			{
				result = float.Parse(size) / 1024;
			}
			else if (bytePower == "GB")
			{
				result = float.Parse(size); ;
			}
			else
			{
				result = -1;
			}

			return result;
		}

		private string getDataBaseSize()
		{
			string getDataBaseSizeCommand = $"SELECT pg_size_pretty( pg_database_size('{this.dataBaseName}') );";
			string size = "";

			this.connection.Open();
			using (var command = new NpgsqlCommand(getDataBaseSizeCommand, this.connection))
			using (var reader = command.ExecuteReader())
			while (reader.Read())
			{
				size = reader.GetString(0);
			}
			this.connection.Close();

			return size;
		}
	}

}