using Microsoft.VisualStudio.TestTools.UnitTesting;
using GetRandomhttpIdentities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetRandomhttpIdentities;

namespace GetRandomhttpIdentities.Tests
{
    [TestClass()]
    public class MyscraperTests
    {
        [TestMethod()]
        [DataRow("Resources\\TopUserAgents.txt")]
        public void WritetoCSVTest(string path)
        {
            var scraper = new Myscraper(); 
            scraper.UserAgents.Add("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36");
            //Act
            scraper.WritetoCSV(path);
           
        }
    }
}