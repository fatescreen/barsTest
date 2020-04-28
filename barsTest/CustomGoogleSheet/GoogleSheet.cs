using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace barsTest.CustomGoogleSheet
{
    class GoogleSheet
    {
        private string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private string spreadsheetId;
        private string sheetName;
        SheetsService service;

        public GoogleSheet(string sheetName, string spreadsheetId)
        {
            this.sheetName = sheetName;
            this.spreadsheetId = spreadsheetId;
        }

        public void updateHeader()
        {
            updateSingleEntry("A1", "Сервер");
            updateSingleEntry("B1", "База данных");
            updateSingleEntry("C1", "Размер в ГБ");
            updateSingleEntry("D1", "Дата обновления");
        }

        public void createSheet(string newSheetName)
        {
            var addSheetRequest = new AddSheetRequest();
            addSheetRequest.Properties = new SheetProperties();
            addSheetRequest.Properties.Title = newSheetName;
            BatchUpdateSpreadsheetRequest batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest();
            batchUpdateSpreadsheetRequest.Requests = new List<Request>();
            batchUpdateSpreadsheetRequest.Requests.Add(new Request { AddSheet = addSheetRequest });

            var batchUpdateRequest = service.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, spreadsheetId);
            batchUpdateRequest.Execute();
        }

        public void updateSingleEntry(string address, string value)
        {
            var range = $"{sheetName}!{address}";
            var valueRange = new ValueRange();

            var objectList = new List<object>() { value };
            valueRange.Values = new List<IList<object>> { objectList };

            var appendRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var appendResponse = appendRequest.Execute();
        }

        public void CreateEntry()
        {
            var range = $"{sheetName}!A:D";
            var valueRange = new ValueRange();
            var objectList = new List<object>() { "test1", "test2", "test3", "test4" };
            valueRange.Values = new List<IList<object>> { objectList};

            var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendResponse = appendRequest.Execute();
        }

        public void testFunction()
        {
            GoogleCredential credential;
            using (var stream = new FileStream("configuration.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                //ApplicationName = ApplicationName,
            });

            var range = $"{sheetName}!A1:D2";
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
