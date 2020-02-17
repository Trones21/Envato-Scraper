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
    public class ReviewTests
    {
        [TestMethod()]
        [DataRow("https://themeforest.net/item/avada-responsive-multipurpose-theme/reviews/2833226", 4)]
        public void SetValuesTest(string URL, int nodeIndex)
        {
            //Arrange Scraper
            var scraper = new Scraper();
            scraper.Request(URL, scraper.Parse);
            scraper.Start();

            //Arrange Obj
            var review = new Review();
            var allNode = scraper.response_t.QuerySelector(".content-s").QuerySelectorAll(".h-mb2");            
            review.SetValues(allNode[nodeIndex], URL);
            Console.WriteLine("Finished!");
        }
    }
}