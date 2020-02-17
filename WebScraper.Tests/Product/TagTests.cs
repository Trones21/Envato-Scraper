using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyWebScraper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebScraper.Tests
{
    [TestClass()]
    public class TagTests
    {

        [TestMethod()]
        [DataRow("https://codecanyon.net/item/visual-composer-page-builder-for-wordpress/242431", "TestroductID")]
        public void DbUpdateLogic(string URL, string ProdID)
        {
            using (var db = new WebScrapeDbContext()) {
                db.Database.ExecuteSqlCommand("delete from [Product].[Tags]");
            }

            //Scrape
            var scraper = new Scraper();
            scraper.Request(URL, scraper.Parse);
            scraper.Start();

            var product = new Product();
            product.SetTags(scraper.response_t, ProdID);
            

            //LogicInApplication(product, ProdID);
            LogicInDB(product, ProdID);
            
                //db.Product_Tag.

                //db.SaveChanges();
                Debug.WriteLine("BP");
            //Clean Table
            //using 
            //db.Database.ExecuteSqlCommand("delete from [Product].[Tags]");
        }
        

        private void LogicInDB(Product product, string ProdID)
        {
           using (var db = new WebScrapeDbContext())
            {
                //db.Database.Exec
            } 
        }

        private void LogicInApplication(Product product, string ProdID)
        {
            var someTags = product.Tags.Take(3);
            //Prepare Table -- Insert a few
            using (var db = new WebScrapeDbContext())
            {
                db.Product_Tag.AddRange(someTags);
                db.SaveChanges();
            }

            using (var db = new WebScrapeDbContext())
            {
                //Act WritetoDB -- Try to Write the rest but skip the ones already written
                var existingTags_p = db.Product_Tag.Where(t => t.ProductID == ProdID)
                                                 .Select(t => new
                                                 {
                                                     t.ProductID,
                                                     t.TagName
                                                 }
                                                     ).ToList();
                var scrapedtags_p = product.Tags.Select(t => new
                {
                    t.ProductID,
                    t.TagName
                }).ToList();

                var newTags = scrapedtags_p.Except(existingTags_p);
            }
        }

        [TestMethod()]
        public void SetValuesTest()
        {
            Assert.Fail();
        }
    }
}