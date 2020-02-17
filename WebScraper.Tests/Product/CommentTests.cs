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
    public class CommentTests
    {
        [TestMethod()]
        [DataRow("https://themeforest.net/item/avada-responsive-multipurpose-theme/2833226/comments", 3)]
        public void SetValuesTest(string URL, int nodeIndex)
        {
            //Arrange Scraper
            var scraper = new Scraper();
            scraper.Request(URL, scraper.Parse);
            scraper.Start();

            //Arrange Obj
            var comment = new Comment();
            var allNode = scraper.response_t.QuerySelectorAll(".js-discussion.comment__container-redesign");
            comment.SetValues(allNode[nodeIndex], URL);
            Console.WriteLine("Finished!");
        }
    }
}