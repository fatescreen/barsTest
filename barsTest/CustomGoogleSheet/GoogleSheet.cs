using barsTest.CustomServer;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;

namespace barsTest.CustomGoogleSheet
{
    /// <summary>
    /// Class for creating google sheets with server/database info
    /// </summary>
    class GoogleSheet : IGoogleSheet
    {
        private string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private string spreadsheetId;
        private string sheetName;
        SheetsService service;
        Server server;
        GoogleCredential credential;
        /// <summary>
        /// Create GoogleSheet with server parameters and database data.
        /// </summary>
        /// <param name="sheetName">name of the sheet</param>
        /// <param name="spreadsheetId">ID of google table</param>
        /// <param name="server">Object with server/database information</param>
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
        /// <summary>
        /// Updates the header of the list. 
        /// </summary>
        public void updateHeader()
        {
            try
            {
                createSheet(this.sheetName);
                updateSingleEntry("A1", "Сервер");
                updateSingleEntry("B1", "База данных");
                updateSingleEntry("C1", "Размер в ГБ");
                updateSingleEntry("D1", "Дата обновления");
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't update sheet header:");
                Console.WriteLine(e.Message);
            }
            
        }
        /// <summary>
        /// Updates the actual data in list.
        /// </summary>
        public void updateData()
        {
            try
            {
                createSheet(this.sheetName);

                DateTime dateTime = DateTime.UtcNow.Date;

                updateSingleEntry("A2", server.Name);
                updateSingleEntry("B2", server.Database);
                updateSingleEntry("C2", server.getDataBaseSizeInGb().ToString());
                updateSingleEntry("D2", dateTime.ToString("dd/MM/yyyy"));
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't update sheet data:");
                Console.WriteLine(e.Message);
            }
            
        }
        /// <summary>
        /// Updates the footer of the list.
        /// </summary>
        public void updateFooter()
        {
            try
            {
                createSheet(this.sheetName);

                DateTime dateTime = DateTime.UtcNow.Date;
                float freeSpace = this.server.getServerSizeInGb() - this.server.getDataBaseSizeInGb();

                updateSingleEntry("A4", server.Name);
                updateSingleEntry("B4", "Свободно");
                updateSingleEntry("C4", freeSpace.ToString());
                updateSingleEntry("D4", dateTime.ToString("dd/MM/yyyy"));
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't update sheet footer:");
                Console.WriteLine(e.Message);
            }            
        }
        /// <summary>
        /// Create new list in current google table. 
        /// 
        /// If sheet with same name is already exists - do nothing.
        /// </summary>
        /// <param name="newSheetName">new list name</param>
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
                //TODO Make another code for case if sheet already exists
            }
            
        }
        /// <summary>
        /// Update single entry in table, in given address with given value.
        /// </summary>
        /// <param name="address">Address to update the value</param>
        /// <param name="value">New value</param> 
        public void updateSingleEntry(string address, string value)
        {
            var range = $"{sheetName}!{address}";
            var valueRange = new ValueRange();

            var objectList = new List<object>() { value };
            valueRange.Values = new List<IList<object>> { objectList };

            var appendRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;

            try
            {
                var appendResponse = appendRequest.Execute();
            }
            catch (Exception e) 
            {
                Console.WriteLine("Can't update single entry:");
                Console.WriteLine(e.Message);
            }
        }        
       
    }
}
