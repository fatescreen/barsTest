using barsTest.CustomServer;
using Google;
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
        Server server;
        GoogleCredential credential;

        public GoogleSheet(string sheetName, string spreadsheetId, Server server)
        {
            this.sheetName = sheetName;
            this.spreadsheetId = spreadsheetId;
            this.server = server;

            using (var stream = new FileStream("configuration.json", FileMode.Open, FileAccess.Read))
            {
                this.credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = this.credential,
            });
        }

        public void updateHeader()
        {
            createSheet(this.sheetName);
            updateSingleEntry("A1", "Сервер");
            updateSingleEntry("B1", "База данных");
            updateSingleEntry("C1", "Размер в ГБ");
            updateSingleEntry("D1", "Дата обновления");
        }

        public void updateData()
        {
            createSheet(this.sheetName);

            DateTime dateTime = DateTime.UtcNow.Date;

            updateSingleEntry("A2", server.Name);
            updateSingleEntry("B2", server.Database);
            updateSingleEntry("C2", server.getDataBaseSizeInGb().ToString());
            updateSingleEntry("D2", dateTime.ToString("dd/MM/yyyy"));
        }

        public void updateFooter()
        {
            createSheet(this.sheetName);

            DateTime dateTime = DateTime.UtcNow.Date;
            float freeSpace = this.server.getServerSizeInGb() - this.server.getDataBaseSizeInGb();

            updateSingleEntry("A4", server.Name);
            updateSingleEntry("B4", "Свободно");
            updateSingleEntry("C4", freeSpace.ToString());
            updateSingleEntry("D4", dateTime.ToString("dd/MM/yyyy"));
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

            try
            {
                batchUpdateRequest.Execute();
            }
            catch (GoogleApiException e)
            { 
                
            }
            
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

       
    }
}
