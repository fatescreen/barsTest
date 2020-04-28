using barsTest.CustomPostgreSQL;

namespace barsTest.CustomServer
{
    /// <summary>
    /// Class that stores server and database information
    /// </summary>
    class Server : IServer
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public float serverSizeInGb { get; set; }

        private string connectionString;      

        public Server()
        {            
            
        }
        /// <summary>
        /// Gets size of database of current server
        /// </summary>
        /// <returns>size of database in GB</returns>
        public float getDataBaseSizeInGb()
        {
            this.connectionString = "Host=" + this.Host +
                                        ";Username=" + this.Username +
                                        ";Password=" + this.Password +
                                        ";Database=" + this.Database;

            PostgreSQL database = new PostgreSQL(this.connectionString);
            return database.getDataBaseSizeInGb();
        }
        /// <summary>
        /// Gets current server size.
        /// </summary>
        /// <returns>server size in GB</returns>
        public float getServerSizeInGb()
        {
            return this.serverSizeInGb;
        }
    }
}
