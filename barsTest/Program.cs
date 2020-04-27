using System.IO;
using System;
using barsTest.CustomPostgreSQL;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data;
using System.Dynamic;

namespace barsTest
{
    class Server
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public string serverSizeInGb { get; set; }        
    }

    class ServerList
    {
        public List<Server> servers { get; set; }
    }

    class Program
    {
        
        JObject jObject;

        //JObject jObject = JObject.Parse(json);
        //JToken jUser = jObject["user"];
        //name = (string) jUser["name"];


        static void Main(string[] args)
        {
            string jsonString;
            using (StreamReader stream = new StreamReader("configuration.json"))
            {
                jsonString = stream.ReadToEnd();                
            }
            ServerList serverList = JsonConvert.DeserializeObject<ServerList>(jsonString);
            Console.WriteLine(serverList.servers[0].Name);

            //foreach (Server server in serverList.list)
            //{
            //    Console.WriteLine(server.Name);
            //}


            //JObject jObject = JObject.Parse(jsonString);
            //JToken jUser = jObject["Server"];
            //string type = (string)jUser["Name"];
            //Console.WriteLine(type);


            string connectionString = "Host=localhost;Username=postgres;Password=lol123;Database=postgres";
            PostgreSQL postgre = new PostgreSQL(connectionString);
            Console.WriteLine(postgre.getDataBaseSizeInGb());

            //GoogleSheet sheet1 = new GoogleSheet();
            //sheet1.testFunction();

            Console.ReadLine();
        }
    }
}
