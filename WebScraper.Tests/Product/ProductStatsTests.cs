using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyWebScraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebScraper.Tests
{
    [TestClass()]
    public class ProductStatsTests
    {
        [DataTestMethod]
        [DataRow("https://graphicriver.net/item/double-exposure-photoshop-action/11319908")]
        public void SetValuesTest(string URL)
        {
            //Arrange Scraper
            var scraper = new Scraper();
            scraper.Request(URL, scraper.Parse);
            scraper.Start();

            //ArrangeObj
            var productstats = new ProductStatsViaPP();
            productstats.SetValues(scraper.response_t);
            
        }
    }
}