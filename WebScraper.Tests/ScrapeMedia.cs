using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IronWebScraper;
using MyWebScraper;
using System.IO;
using System.Diagnostics;

namespace WebScraper.Tests
{
    [TestClass]
    public class ScrapeMedia
    {
        [TestMethod()]
        [DataRow("https://s3.envato.com/files/255493339/heron-hunting-fish-vh.png")]
        public void ScrapeImage(string URL)
        {            
            var scraper = new Scraper();
            //Need to save as the Username
            scraper.DownloadImage(URL, ".");
            scraper.Start();
            DirectoryInfo dir = new DirectoryInfo(".");
            Debug.WriteLine(dir.FullName);
        }
    }
}
