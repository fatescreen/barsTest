using System.IO;
using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Npgsql;
using System;


namespace barsTest
{
    class Program
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };

        static readonly string ApplicationName = "Gtable";

        static readonly string SpreadsheetId = "1ZDRmLulyTT1okyM55q4MxJXP6xfLUDLHupH7SIPwda0";

        static readonly string sheet = "list1";

        static SheetsService service;

        static void Main(string[] args)
        {

            GoogleCredential credential;
            using (var stream = new FileStream("gclient_keys.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            var range = $"{sheet}!A1:F10";
            var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);

            var response = request.Execute();
            var values = response.Values;

            foreach (var row in values)
            {
                Console.WriteLine("{0} {1} {2}", row[0], row[1], row[2]);
            }

                var cs = "Host=localhost;Username=postgres;Password=lol123;Database=postgres";

            using var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = "SELECT * from test";

            using var cmd = new NpgsqlCommand(sql, con);

            var version = cmd.ExecuteScalar().ToString();
            Console.WriteLine($"PostgreSQL version: {version}");
            Console.ReadLine();
        }
    }
}
