using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;

namespace MyWebScraper
{
    /// <summary>
    /// Use the response_t property of this class in combination with the Method I want to Test
    /// </summary>
    public class Scraper : WebScraper
    {
        public Response response_t = new Response();
        public int WritelineEach;
        public List<Product> Products = new List<Product>();
        public List<Review> Reviews = new List<Review>();
        public List<Comment> Comments = new List<Comment>();
        public override void Init()
        {
            this.Identities = Program.HttpIdentities();
            this.LoggingLevel = LogLevel.Critical;
        }
        /// <summary>
        /// When I want to Save a specific response
        /// </summary>
        /// <param name="response"></param>
        public override void Parse(Response response)
        {
            this.response_t = response;
        }
        internal void ParseWithCounter(Response response)
        {
            
            if(SuccessfulfulRequestCount%WritelineEach == 0)
            {
                Console.WriteLine(SuccessfulfulRequestCount + " requests made");
            }
        }
        public void PortfolioPageScrape(Response response)
        {
            Debug.WriteLine(response.FinalUrl);
            var AuthorName = response.QuerySelector(".user-info-header__content > a > h2").InnerText.Trim();            
            var productNodes = response.QuerySelector(".product-grid").QuerySelectorAll(".js-google-analytics__list-event-container");
            foreach (var node in productNodes)
            {
                var product = new Product();
                product.SetValuesViaList(node, AuthorName);
                Products.Add(product);               
            }            
        }
        public void ReviewsPageScrape(Response response)
        {
            Debug.WriteLine(response.FinalUrl);
            var ReviewNodes = response.QuerySelectorAll("");
            var ProductID = response.FinalUrl.Split('?')[0].Split('/').Last();
            
           foreach (var node in ReviewNodes)
            {
                var review = new Review();
                review.SetValues(node, ProductID);
                Reviews.Add(review);
            }           
        }
        public void CommentsPageScrape(Response response)
        {
            Debug.WriteLine(response.FinalUrl);
            var CommentNodes = response.QuerySelectorAll("");
            var ProductID = response.FinalUrl.Split('?')[0].Split('/')
                [response.FinalUrl.Split('?')[0].Split('/').Length - 2];

            
            foreach (var node in CommentNodes)
            {
                var comment = new Comment();
                comment.SetValues(node, ProductID);
                Comments.Add(comment);
            }
        }
    }
}
