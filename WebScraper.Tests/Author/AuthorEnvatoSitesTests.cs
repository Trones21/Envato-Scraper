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
    public class AuthorEnvatoSitesTests
    {
        [TestMethod()]
        public void RetrieveEnvatoSitesTest()
        {
            //Arrange Scraper
            var scraper = new Scraper();
            scraper.Request("https://3docean.net/user/paguthrie", scraper.Parse);
            scraper.Start();

            //Arrange Test Obj
            var authorSites = new AuthorEnvatoSites();
            //Act
            authorSites.SetValues(scraper.response_t);

            //Assert
            //PK_Later -- How and what should I check?
        }
    }
}