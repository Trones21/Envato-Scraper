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
    public class AuthorSocialSitesTests
    {
        [TestMethod()]
        [DataRow("https://graphicriver.net/user/zeisla")]
        public void RetrieveSocialNetworksTest(string URL)
        {
            //Arrange Scraper
            var scraper = new Scraper();
            scraper.Request(URL, scraper.Parse);
            scraper.Start();

            //Arrange Test 
            var authorSites = new AuthorSocialSites();
            //Act         
            authorSites.SetValues(scraper.response_t);
            Console.WriteLine("Finished!");
            
        }
    }
}