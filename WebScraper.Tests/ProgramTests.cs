using GetRandomhttpIdentities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyWebScraper;
using MyWebScraper._Generic;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MyWebScraper.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        public List<string> Usernames = new List<string>();
        private List<string> RankUsername = new List<string>();

        [TestMethod()]
        public void CreateUserAgentsFileforTests()
        {
            ScrapeRandomHttpIdentities.Main("Resources\\TopUserAgents.txt");
        }

        [TestMethod()]
        [Ignore]
        [DataRow(123, 132)]
        public void CacheWarmingtoGetAll(int startPage, int endPage)
        {
            //Arrange Math
            int expectedResults = (endPage + 1 - startPage) * 50;
            int firstRank = (startPage * 50) - 49;
            int lastRank = (endPage * 50);

            //Arrange Scraper
            var cacheWarmer = new Scraper();
            for (int page = startPage; page <= endPage; page++)
            {
                cacheWarmer.Request("https://themeforest.net/authors/top?level=all&site=all&page=" + page, cacheWarmer.Parse);
            }
            cacheWarmer.Start();

            //Act                      
            var scraper = new Scraper();

            for (int page = startPage; page <= endPage; page++)
            {
                scraper.Request("https://themeforest.net/authors/top?level=all&site=all&page=" + page, this.GrabName);
            }
            scraper.Start();

            //Get Distinct Usernames
            var UserNamesDistinct = this.Usernames.Distinct();

            //Assert

            Console.WriteLine("Count: " + Usernames.Count);
            Console.WriteLine("Distinct Count: " + UserNamesDistinct.Count());
            Console.WriteLine("Expected: " + expectedResults);
            foreach (var RankUser in RankUsername)
            {
                Console.WriteLine(RankUser);
            }
            Assert.AreEqual(Usernames.Count, UserNamesDistinct.Count());
        }
        public void GrabName(IronWebScraper.Response response)
        {
            var usernameNodes = response.QuerySelectorAll(".user-info__username");
            foreach (var node in usernameNodes)
            {
                this.Usernames.Add(node.QuerySelector("a").GetAttribute("href"));
                this.RankUsername.Add(node.QuerySelector("a").InnerText);
            }

        }


        public void ToCsvIgnoreListsTest()
        {
            throw new NotImplementedException();
            //Arrange           
            var separator = ",";

            var expected = new List<string>();
            expected.Add("Name,URL,itemsCount,MinPrice,MaxPrice");
            expected.Add(",,0,0,51");
            var exp = string.Join("|", expected);

            var objectlist = new List<ProductCategoryStats>();
            for (int i = 0; i < 1; i++)
            {
                var obj = new ProductCategoryStats();
                obj.MaxPrice = 51;
                //obj.Tags = new Dictionary<string, int>();
                //obj.Tags.Add("myStr", 4);
                objectlist.Add(obj);
            }

            //Act
            IEnumerable<string> result = UnsortedMethods.ToCsvIgnoreLists(separator, objectlist);
            var actual = result.ToList();
            var act = string.Join("|", actual);
            Console.WriteLine(exp);
            Console.WriteLine(act);

            //Assert
            Assert.AreEqual(exp, act);

        }

        [TestMethod()]
        public void CountInsertedTodayTest()
        {
            var dbcontext = new WebScrapeDbContext();
            string SchemaTable = "[Author].[StatsViaList]";
            string DateColumn = "[Date]";
            var cntinserted = ParameterizedQueries.CountInsertedToday(dbcontext, SchemaTable, DateColumn);
            Console.WriteLine(cntinserted);
        }
    }
}