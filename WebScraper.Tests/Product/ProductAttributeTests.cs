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
    public class ProductAttributeTests
    {
        [TestMethod()]
        [DataRow("https://codecanyon.net/item/visual-composer-page-builder-for-wordpress/242431", "242431", 6)]
        public void SetValuesTest(string URL, string productID, int nodeIndex)
        {
            //Arrange
            var scraper = new Scraper();
            scraper.Request(URL, scraper.Parse);
            scraper.Start();                     
            var node = scraper.response_t.QuerySelector(".meta-attributes").QuerySelectorAll("tr")[nodeIndex];

            var prodattr = new ProductAttribute();

            //Act
            prodattr.SetValues(node, productID);
            Console.WriteLine("Finished!");
        }
    }
}