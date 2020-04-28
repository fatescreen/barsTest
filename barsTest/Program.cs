using barsTest.CustomGoogleTable;
using System;

namespace barsTest
{    
    class Program
    {       
        static void Main(string[] args)
        {            
            GoogleTable googleTable = new GoogleTable("configuration.json");
            googleTable.foreverUpdateStart(600);
            Console.ReadLine();
        }                
    }
}
