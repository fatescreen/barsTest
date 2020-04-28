
using barsTest.CustomGoogleSheet;
using barsTest.CustomServer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace barsTest.CustomGoogleTable
{
    class GoogleTable : IGoogleTable
    {
        List<GoogleSheet> googleSheetList;
        ServerList serverList;
        public SpreadsheetId spreadsheetId;
        System.Timers.Timer aTimer;

        public GoogleTable(string configuration)
        {
            googleSheetList = new List<GoogleSheet>();
            
            string jsonString;
            using (StreamReader stream = new StreamReader("configuration.json"))
            {
                jsonString = stream.ReadToEnd();
            }
            this.serverList = JsonConvert.DeserializeObject<ServerList>(jsonString);
            this.spreadsheetId = JsonConvert.DeserializeObject<SpreadsheetId>(jsonString);            

            foreach (Server server in serverList.servers)
            {                
                GoogleSheet googleSheet = new GoogleSheet(server.Name, this.spreadsheetId.get(), server);
                this.googleSheetList.Add(googleSheet);
            }
        }

        public void update()
        {
            foreach (GoogleSheet googleSheet in this.googleSheetList)
            {
                googleSheet.updateHeader();
                googleSheet.updateData();
                googleSheet.updateFooter();
            }
        }

        public void foreverUpdateStart(long intervalTimeInSec)
        {
            timerConfiguration(intervalTimeInSec);
            this.aTimer.Enabled = true;
        }

        public void foreverUpdateStop()
        {
            this.aTimer.Enabled = false;
        }

        private void timerConfiguration(long intervalTimeInSec)
        {
            this.aTimer = new System.Timers.Timer();

            var intervalTimeInMs = intervalTimeInSec * 1000;
            this.aTimer.Interval = intervalTimeInMs;
            this.aTimer.Elapsed += OnTimedEvent;
            this.aTimer.AutoReset = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            update();
        }
    }

    class SpreadsheetId 
    {
        public string googleSpreadsheetId { get; set; }

        public string get()
        {
            return this.googleSpreadsheetId;
        }
    }
}
