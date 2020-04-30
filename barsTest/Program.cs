using barsTest.CustomGoogleTable;
using System;

namespace barsTest
{    
    class Program
    {       
        static void Main(string[] args)
        {            
            GoogleTable googleTable = new GoogleTable("configuration.json");
            Console.WriteLine("Application is running...");
            googleTable.foreverUpdateStart(15);
            Console.WriteLine("Google table created...");
            Console.ReadLine();
        }                
    }
}
