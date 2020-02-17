using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;

namespace MyWebScraper
{
    /// <summary>
    /// Depends on:
    /// <para>List of Links to Author Portfolio Pages</para>
    /// </summary>
    public class PortfolioScraper : WebScraper
    {
        //It looks like the portfolio tab won't show up unless you are on the proper site -- i.e. yogurt86 has no portfolio tab on 
        //VideoHive, but he does have one on GraphicRiver.
        //The top authors page does not change the domain, so we must follow the link X <Site> items
        //ex. https://codecanyon.net/user/wpbakery/portfolio?sso=1&_ga=2.6730034.1092727548.1539855106-1651659662.1536927686
        //Remove Everything after the ?
        string CurrentAuthor;
        int productPages;
        string ReferringLink;
        string newestProductinDB;
        

        //The product rating is only in .5 increments on the portfolio page -- ex. 4.5 instead of 4.65
        //I wonder how this is rounded.

        public override void Init()
        {
            this.Identities = Program.HttpIdentities();
            this.LoggingLevel = WebScraper.LogLevel.All;
            Main();
        }
        public void ParsePageCount(Response response)
        {
            if (response.CssExists(".pagination__summary"))
            {
                var rawString = response.Css(".pagination__summary")[0].InnerTextClean;
                productPages = Convert.ToInt32(rawString.Split(' ').Last().Trim());
            }
        }

        public void Main()
        {



        }

        public void PerAuthor()
        {
            //Don't need to check this when Author = New (Join Author and Product and then get count)

            this.Request("", ParsePageCount);

            //PK_Woulld like to figure out how to do this var newProductLinks this.Request(baseURL + "/user/" + CurrentAuthor + "/portfolio?direction=desc", GetNewProductsfromPortfolio);

            //PK_BuildComment foreach (string productlink in newProductLinks)
            {
                this.Request("", Parse);
            }
        }

        public void GetNewestProductfromDB(string AuthorID)
        {
            //Select top 1
            //Name
            //from Products 
            //where AuthorID = AuthorID AND Date = MAX(Date)
            newestProductinDB = "XXX";
        }
        /*PK_Build Comment
        /// <summary> Notes
        ///If I order by date descending, then I only need to hit this page once 
        ///I can skip this altogether if the number of items is the same as it was the day before
        ///Since the date published does not exist on the webpage, I will have to conpare the names
        ///if newestProduct = Product, then stop scraping
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public void GetNewProductsfromPortfolio(Response response, out List<string> newProductLinks)
        {
             
           var products = response.QuerySelectorAll(".js-google-analytics__list-event-container");
            List<string> newProductLinks = new List<string>();
            foreach (var product in products)
            {
                if(product.QuerySelector(".product-list__heading").InnerText == newestProductinDB)
                {
                    out newProductLinks;
                }
                else
                {
                    //Add Product Link
                    var link = product.QuerySelector(".product-list__heading").GetElementsByTagName("a")[0].GetAttribute("href");
                    newProductLinks.Add(link);
                }
            }
            out newProductLinks;

        }
        */
        public override void Parse(Response response)
        {
            Product ProductweUpdate = new Product();
            ProductweUpdate.SetValues(response);
        }

        
        
    }
}
