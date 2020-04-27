using System.IO;
using System;
using barsTest.CustomPostgreSQL;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data;
using System.Dynamic;
using barsTest.CustomGoogleSheet;
using barsTest.CustomServer;

namespace barsTest
{
    
    class Program
    {
        
        static void Main(string[] args)
        {
            string jsonString;
            using (StreamReader stream = new StreamReader("configuration.json"))
            {
                jsonString = stream.ReadToEnd();                
            }
            ServerList serverList = JsonConvert.DeserializeObject<ServerList>(jsonString);
            Console.WriteLine(serverList.servers[0].Name);
            Console.WriteLine(serverList.servers[0].getDataBaseSizeInGb());
            Console.WriteLine(serverList.servers[0].getServerSizeInGb());


            string spreadsheetId = "1ZDRmLulyTT1okyM55q4MxJXP6xfLUDLHupH7SIPwda0";
            string sheetName = "list1";
            GoogleSheet sheet1 = new GoogleSheet(sheetName, spreadsheetId);
            sheet1.testFunction();

            Console.ReadLine();
        }
    }
}
