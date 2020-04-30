
using barsTest.CustomGoogleSheet;
using barsTest.CustomServer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace barsTest.CustomGoogleTable
{
    /// <summary>
    /// Class that consits of GoogleSheet's objects
    /// </summary>
    class GoogleTable : IGoogleTable
    {
        List<GoogleSheet> googleSheetList;
        ServerList serverList;
        public SpreadsheetId spreadsheetId;
        System.Timers.Timer aTimer;
        /// <summary>
        /// Creates GoogleTable wiht lists of servers, that described in .json configuration file.
        /// </summary>
        /// <param name="configuration">name of configuration .json file, that lay in project directory</param>
        public GoogleTable(string configuration)
        {
            googleSheetList = new List<GoogleSheet>();
            
            string jsonString;            
            using (StreamReader stream = new StreamReader(configuration))
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

        /// <summary>
        /// Update all parts of google sheet
        /// </summary>
        public void update()
        {
            foreach (GoogleSheet googleSheet in this.googleSheetList)
            {
                googleSheet.updateHeader();
                googleSheet.updateData();
                googleSheet.updateFooter();
            }
        }
        /// <summary>
        /// Starts forever updating of google sheets with choosen time interval
        /// </summary>
        /// <param name="intervalTimeInSec">Interval for update in seconds</param>
        public void foreverUpdateStart(long intervalTimeInSec)
        {
            update();
            timerConfiguration(intervalTimeInSec);
            this.aTimer.Enabled = true;
        }
        /// <summary>
        /// Stops forever updating
        /// </summary>
        public void foreverUpdateStop()
        {
            this.aTimer.Enabled = false;
        }
        /// <summary>
        /// Configurating timer for forever updating
        /// </summary>
        /// <param name="intervalTimeInSec">Interval time in seconds</param>
        private void timerConfiguration(long intervalTimeInSec)
        {
            this.aTimer = new System.Timers.Timer();

            var intervalTimeInMs = intervalTimeInSec * 1000;
            this.aTimer.Interval = intervalTimeInMs;
            this.aTimer.Elapsed += OnTimedEvent;
            this.aTimer.AutoReset = true;
        }
        /// <summary>
        /// Callback function for timer.
        /// </summary>
        /// <param name="sender">Link to sender object</param>
        /// <param name="e">Time argument</param>
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            update();
            Console.WriteLine("Google table updated...");
        }
    }
    /// <summary>
    /// Class for easy deserializing googleSpreadsheetId from .json file
    /// </summary>
    class SpreadsheetId 
    {
        public string googleSpreadsheetId { get; set; }

        public string get()
        {
            return this.googleSpreadsheetId;
        }
    }
}
