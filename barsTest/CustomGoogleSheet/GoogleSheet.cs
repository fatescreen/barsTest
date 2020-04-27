using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace barsTest.CustomGoogleSheet
{
    class GoogleSheet
    {
        private string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private string spreadsheetId = "1ZDRmLulyTT1okyM55q4MxJXP6xfLUDLHupH7SIPwda0";
        private string sheetName = "list1";
        SheetsService service;

        public GoogleSheet(string sheetName, string spreadsheetId)
        {
            this.sheetName = sheetName;
            this.spreadsheetId = spreadsheetId;
        }

        public void testFunction()
        {
            GoogleCredential credential;
            using (var stream = new FileStream("gclient_keys.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                //ApplicationName = ApplicationName,
            });

            var range = $"{sheetName}!A1:F10";
            var request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            var response = request.Execute();
            var values = response.Values;

            foreach (var row in values)
            {
                Console.WriteLine("{0} {1} {2}", row[0], row[1], row[2]);
            }

        }
    }
}
