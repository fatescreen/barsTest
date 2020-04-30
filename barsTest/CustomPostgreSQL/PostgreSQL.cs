using Newtonsoft.Json.Linq;
using Npgsql;
using System;

namespace barsTest.CustomPostgreSQL
{
	/// <summary>
	/// Class for getting data from postgreSQL database
	/// </summary>
	public class PostgreSQL : IPostgreSQL
	{
		private string connectionString;
		private NpgsqlConnection connection;
		private string dataBaseName;

		/// <summary>
		/// Creates object of postgre database connection and stores some data from this database
		/// </summary>
		/// <param name="connectionString"></param>
		public PostgreSQL(string connectionString)
		{
			this.connectionString = connectionString;
			this.connection = new NpgsqlConnection(this.connectionString);
			this.dataBaseName = connectionString.Substring(connectionString.IndexOf("Database=")).Replace("Database=", "");
		}
		/// <summary>
		/// Gets database size in GB.
		/// </summary>
		/// <returns>size in GB</returns>
		public float getDataBaseSizeInGb()
		{
			float result = 0;
			string[] sizeInTwoWords = null;

			try
			{
				sizeInTwoWords = getDataBaseSize().Split(' ');
			}
			catch (Exception e)
			{
				Console.WriteLine("Can't get PostgreSQL database size:");
				Console.WriteLine(e.Message);
			}

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
				result = float.Parse(size);
			}
			else
			{
				result = -1;
			}

			return result;
		}
		/// <summary>
		/// Gets database size in string format in kB, MB, or GB.
		/// </summary>
		/// <returns>database size, for example: "8463 kB"</returns>
		private string getDataBaseSize()
		{
			string getDataBaseSizeCommand = $"SELECT pg_size_pretty( pg_database_size('{this.dataBaseName}') );";
			string size = "";

			try
			{
				this.connection.Open();
			}
			catch (Exception e)
			{
				Console.WriteLine("Can't open connection to PostgreSQL database:");
				Console.WriteLine(e.Message);
			}

			try
			{
				using (var command = new NpgsqlCommand(getDataBaseSizeCommand, this.connection))
				using (var reader = command.ExecuteReader())
					while (reader.Read())
					{
						size = reader.GetString(0);
					}
			}
			catch (Exception e)
			{
				Console.WriteLine("Can't get PostgreSQL database size:");
				Console.WriteLine(e.Message);
			}

			try
			{
				this.connection.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine("Can't close connection to PostgreSQL database:");
				Console.WriteLine(e.Message);
			}			

			return size;
		}
	}

}