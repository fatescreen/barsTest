using barsTest.CustomPostgreSQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace barsTest.CustomServer
{
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

        public float getDataBaseSizeInGb()
        {
            this.connectionString = "Host=" + this.Host +
                                        ";Username=" + this.Username +
                                        ";Password=" + this.Password +
                                        ";Database=" + this.Database;

            PostgreSQL database = new PostgreSQL(this.connectionString);
            return database.getDataBaseSizeInGb();
        }

        public float getServerSizeInGb()
        {
            return this.serverSizeInGb;
        }
    }
}
