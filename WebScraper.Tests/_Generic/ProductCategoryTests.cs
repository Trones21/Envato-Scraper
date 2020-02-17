using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyWebScraper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraper.Tests;

namespace MyWebScraper.Tests
{
    [TestClass()]
    public class ProductCategoryTests
    {
        [TestMethod()]
        [DataRow("https://audiojungle.net/category/all#content")]
        public void SetValuesTest(string URL)
        {
            //Arrange
            var response = Methods.MakeRequest(URL);
            var productCategoryStats = new ProductCategoryStats();
            var filterspanel = response.QuerySelector("div[data-test-selector=search-filters]");

            //Act
            productCategoryStats.SetValues(filterspanel, URL);

            //Assert
            Console.WriteLine("BP");
            //foreach (var property in productCategoryStats.GetType().GetProperties())
            //{
            //    Debug.WriteLine(property);
            //}
            //foreach (var nestedType in productCategoryStats.GetType().GetNestedTypes())
            //{
            //    nestedType.GetProperties();
            //}
        }

        public void HaveFiltersChanged()
        {
            
        }

        [TestMethod()]
        public void ListAllFilters()
        {
            var sites = new List<string>
            {
                "https://audiojungle.net/category/all#content",
                "https://themeforest.net/category/all#content",
                "https://codecanyon.net/category/all#content",
                "https://videohive.net/category/all#content",
                "https://graphicriver.net/all-graphics#content",
                "https://photodune.net/category/all#content",
                "https://3docean.net/category/all#content"
            };

            var siteFilters = new List<SiteFilter>();

            foreach (var site in sites)
            {
                siteFilters.AddRange(CategoryStatsScraper.GetFilters(Methods.MakeRequest(site)));                              
            }

            foreach (var filter in siteFilters)
            {
                Debug.WriteLine(filter.FilterName   + "   ---   " + filter.URL);
            }

            Console.WriteLine("BP");

        }
                
    }
}