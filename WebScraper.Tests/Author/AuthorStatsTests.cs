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
    public class AuthorStatsTests
    {
        [TestMethod()]
        [DataRow("https://graphicriver.net/authors/top?page=5", 7)]
        public void SetViaTopListTest(string URL, int nodeIndex)
        {
            //Arrange Scraper
            var scraper = new Scraper();
            scraper.Request(URL, scraper.Parse);
            scraper.Start();

            //Arrange Test 
            var authorstats = new AuthorStatsViaList();
            var htmlnode = scraper.response_t.QuerySelectorAll(".user-list > li")[nodeIndex];
            authorstats.SetValuesViaList(htmlnode);
            Console.WriteLine("Finished");
        }

        [TestMethod()]
        [DataRow("https://videohive.net/user/mrikhokon")]
        public void SetValuesViaProfilePageTest(string URL)
        {
            //Arrange
            var scraper = new Scraper();
            scraper.Request(URL, scraper.Parse);
            scraper.Start();

            //Act
            var authorstatsviaPP = new AuthorStatsViaPP();
            authorstatsviaPP.SetValuesViaProfilePage(scraper.response_t);
            Console.WriteLine("Finished!");

        }
    }
}