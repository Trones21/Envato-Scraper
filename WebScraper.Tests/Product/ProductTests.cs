using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyWebScraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;
using WebScraper.Tests;

namespace MyWebScraper.Tests
{
    [TestClass()]
    public class ProductTests
    {
        [TestMethod()]
        [DataRow("https://videohive.net/item/handy-seamless-transitions-pack-script/18967340")]
        public void Details_Reviews_Comments(string ProductURL)
        {
            var scraper = new Scraper();
            var product = new Product();
            product.reviewsPagesCnt = 5;
            product.commentsPagesCnt = 5;


            scraper.Request(ProductURL, scraper.Parse); //Product Page
            scraper.Start();
            product.SetValues(scraper.response_t);
            
            product.Construct_commentsbaseURL();
            product.Construct_reviewsbaseURL();

            if (product.hasReviews == true)
            {
                for (int page = 1; page <= product.reviewsPagesCnt; page++)
                {
                    scraper.Request(product.reviewsbaseURL + "?page=" + page, scraper.ReviewsPageScrape);
                }
            }

            if (product.hasComments == true)
            {
                for (int page = 1; page <= product.commentsPagesCnt; page++)
                {
                    scraper.Request(product.commentsbaseURL + "?page=" + page, scraper.CommentsPageScrape);
                }
            }
            scraper.Start();
        }



        [TestMethod()]
        [DataRow("https://codecanyon.net/item/visual-composer-page-builder-for-wordpress/242431")]
        public void SetValuesTest(string URL)
        {
            //Arrange
            var scraper = new Scraper();
            scraper.Request(URL, scraper.Parse);
            scraper.Start();

            //Arrange Obj
            var product = new Product();
            product.SetValues(scraper.response_t);
            Console.WriteLine("Finished");

        }

        [TestMethod()]
        [DataRow("https://videohive.net/item/handy-seamless-transitions-pack-script/18967340")]
        public void Calc_GETreq_costTest(string ProductURL)
        {
            //Act
            var product = new Product();
            product.Calculate_GETreq_cost(ProductURL);

            Console.WriteLine(product.reviewsPagesCnt);
            Console.WriteLine(product.commentsPagesCnt);
            Console.WriteLine(product.GETreq_fullcost);
        }


        [TestMethod()]
        [DataRow("https://audiojungle.net/item/energetic-upbeat-pop/20040701", "20040701", 5)]
        [DataRow("https://videohive.net/item/handy-seamless-transitions-pack-script/18967340", "18967340", 2)]
        public void SetLicenseValuesTest(string URL, string ID, int licenseCount)
        {

            //Arrange
            var response = Methods.MakeRequest(URL);

            var product = new Product();

            //Prepare Parameters
            var node = response.QuerySelector("#purchase-form > form > div > div:nth-child(4)");

            //Act
            product.SetLicenseValues(node);
            product.licenses = product.licenses.Select(e => { var ret = e; e.ProductID = ID; return e; }).ToList<License>();
            //Assert
            foreach (var lic in product.licenses)
            {
                Console.WriteLine("-----License---------");
                Console.WriteLine(lic.LicenseName);
                Console.WriteLine(lic.Price);
                Console.WriteLine("--------------------");

            }

            Assert.AreEqual(licenseCount, product.licenses.Count);
        }

        [TestMethod()]
        [DataRow("https://videohive.net/item/handy-seamless-transitions-pack-script/18967340",
            "https://videohive.net/item/handy-seamless-transitions-pack-script/reviews/18967340")]
        public void Construct_reviewsbaseURLTest(string URL, string expected)
        {
            //Arrange
            var product = new Product();
            product.URL = URL;

            //Act
            product.Construct_reviewsbaseURL();

            //Assert
            Assert.AreEqual(expected, product.reviewsbaseURL);


        }

        [TestMethod()]
        [DataRow("https://videohive.net/item/handy-seamless-transitions-pack-script/18967340", true)]
        public void set_hasReviewsTest(string URL , bool expected)
        {
            var response = Methods.MakeRequest(URL);
            var product = new Product();
            product.set_hasReviews(response);
            Assert.AreEqual(expected, product.hasReviews);
        }

        [TestMethod()]
        [DataRow("https://videohive.net/item/handy-seamless-transitions-pack-script/18967340", true)]
        public void set_hasCommentsTest(string URL, bool expected)
        {
            var response = Methods.MakeRequest(URL);
            var product = new Product();
            product.set_hasComments(response);
            Assert.AreEqual(expected, product.hasComments);
        }
    }
}