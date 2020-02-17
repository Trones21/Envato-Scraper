using IronWebScraper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace MyWebScraper
{
    /// <Summary>
    ///
    /// </Summary> 
    [Table("Products", Schema = "Product")]
    public class Product
    {
        //Set from DB & Site
        bool HasNewReviews;
        bool HasNewComments;

        //Set from DB
        int previousCommentCount;
        int previousReviewCount;

        //Set from Site & Write ALL to DB
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ProductID { get; set; } //Derived from the Link -- This is Envato's Product ID
        public string Title { get; set; }
        public string Author { get; set; }
        public string Date_CreatedbyAuthor { get; set; }
        [Column(TypeName = "Date")]
        public DateTime Date_LastUpdatedbyAuthor { get; set; }           
        public string Hierarchy { get; set; }
        public string URL { get; set; } //This gives me the site, ID, and the URL formatted name 
        public string logoURL { get; set; }
        public int ProductDetailsMaxChangeSetID { get; set; }
        public int ProductAttributesMaxChangeSetID { get; set; }
        
        //Set via DB
        public int GETreq_fullcost { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? Date_ofInitialFullScrape { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? Date_LastSupplementalScrape { get; set; }
        
        /// <summary>
        /// Set via Item Details Page
        /// </summary>
        public bool? hasComments { get; set; }
        public void set_hasComments(Response response)
        {
            var tabs = response.QuerySelector(".page-tabs").GetElementsByTagName("li");
            hasComments = false;
            foreach (var tab in tabs)
            {
                switch (tab)
                {
                    case var match when (tab.QuerySelector("a").InnerText.Trim() == "Comments"):
                        commentsbaseURL = tab.QuerySelector("a").GetAttribute("href");
                        hasComments = true;
                        break;
                }
            }
        }
        public bool? hasReviews { get; set; }
        public void set_hasReviews(Response response)
        {
            var tabs = response.QuerySelector(".page-tabs").GetElementsByTagName("li");
            hasReviews = false;
            foreach (var tab in tabs)
            {
                switch (tab)
                {
                    case var match when (tab.QuerySelector("a").InnerText.Trim() == "Reviews"):
                        reviewsbaseURL = tab.QuerySelector("a").GetAttribute("href");
                        hasReviews = true;
                        break;
                }
            }
        }


        public ProductDetailsChangeSet productdetails = new ProductDetailsChangeSet();
        public ProductStatsViaPP productStats = new ProductStatsViaPP();
        public List<ProductAttribute> productAttributes = new List<ProductAttribute>();
        public void SetAttributes(Response response)
        {
            var AllNodes = response.QuerySelector(".meta-attributes__table > tbody").QuerySelectorAll("tr");
            var nodestoExclude = new List<string> { "Last Update", "Created", "Tags" };
            var filteredNodes = new List<HtmlNode>();
            foreach (var node in AllNodes)
            {
                if (!nodestoExclude.Contains(node.QuerySelector(".meta-attributes__attr-name").InnerText))
                {
                    filteredNodes.Add(node);
                }
            }
            foreach (var node in filteredNodes)
            {
                var attribute = new ProductAttribute();
                attribute.SetValues(node, this.ProductID);
                this.productAttributes.Add(attribute);
            }
        }

        public List<License> licenses = new List<License>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParamNode">"div data-view=itemVariantSelector"</param>
        public void SetLicenseValues(HtmlNode ParamNode)
        {

            var nodes = ParamNode.ChildNodes;
            var filteredNodes = nodes.Where(n => n.HasAtribute("data-license")).ToList<HtmlNode>();
            
            foreach (var node in filteredNodes)
            {
                var lic = new License();
                lic.SetValues(node);
                this.licenses.Add(lic);
            }
        }

        public List<Tag> Tags = new List<Tag>();
        public void SetTags(Response response, string ProductID)
        {
            var Nodes = response.QuerySelector(".meta-attributes__attr-tags").QuerySelectorAll("a");
            foreach (var node in Nodes)
            {
                var tag = new Tag();
                tag.SetValues(node, ProductID);
                Tags.Add(tag);
            }
        }

        /// <summary>
        /// // ---Scraping Note: These come from other pages
        /// </summary>
        List<Review> Reviews = new List<Review>();
        public string reviewsbaseURL;
        public int reviewsPagesCnt;
        public void Construct_reviewsbaseURL()
        {
            var partialURL = URL.Substring(0, URL.LastIndexOf('/'));

            var productID = URL.Split('/').Last();
            reviewsbaseURL = partialURL + "/reviews/" + productID;
        }
        
        /// <summary>
        /// // ---Scraping Note: These come from other pages
        /// </summary>
        List<Comment> Comments = new List<Comment>();
        public string commentsbaseURL;
        public int commentsPagesCnt;
        public void Construct_commentsbaseURL()
        {
            commentsbaseURL = URL + "/comments";
        }


        public void SetValues(Response response)
        {
            #region NewProducts 
            //this.Link;
            //this.AuthorID
            this.URL = response.RequestlUrl;

            //Get the HierarchyLink -- I may want to put reoccuring strings in another table later, just so the DB doesn't get too big
            var node = response.Css(".context-header")[0].ChildNodes[1].ChildNodes[1];
            var lastNode = node.QuerySelectorAll("A").Length - 1;
            this.Hierarchy = node.QuerySelectorAll("A")[lastNode].GetAttribute("href");
            
            #endregion
            //Attributes Table
            var metaAttr = response.QuerySelector(".meta-attributes__table");
            this.Date_CreatedbyAuthor = metaAttr.QuerySelector(".updated").InnerText;

            logoURL = response.QuerySelector(".grid-container > link").GetAttribute("href");

            this.set_hasComments(response);
            this.set_hasReviews(response);

        }
        public void Calculate_GETreq_cost(string ProductURL)
        {
            //Setup
            var scraper = new Scraper();
            scraper.Request(ProductURL, scraper.Parse);
            scraper.Start();
            
            //Get ProductID just in case it hasn't been set
            ProductID = scraper.response_t.RequestlUrl.Split('/').Last().Trim();

            var idx = ProductURL.IndexOf("item");
            //var baseURL = ProductURL.Substring(0, idx - 1);


            var tabs = scraper.response_t.QuerySelector(".page-tabs").GetElementsByTagName("li");
            foreach (var tab in tabs)
            {
                switch (tab)
                {
                    case var match when (tab.QuerySelector("a").InnerText.Trim() == "Comments"):
                        commentsbaseURL = tab.QuerySelector("a").GetAttribute("href");
                        hasComments = true;
                        break;
                    case var match when (tab.QuerySelector("a").InnerText.Trim() == "Reviews"):
                        reviewsbaseURL = tab.QuerySelector("a").GetAttribute("href");
                        hasReviews = true;
                        break;
                    default: break;
                }
            }

            //Store Responses
            var getReviewsPage = new Scraper();
            getReviewsPage.Request(reviewsbaseURL, getReviewsPage.Parse);
            getReviewsPage.Start();
            var reviewsPage = getReviewsPage.response_t;
            

            var getCommentsPage = new Scraper();
            getCommentsPage.Request(commentsbaseURL, getCommentsPage.Parse);
            getCommentsPage.Start();
            var commentsPage = getCommentsPage.response_t;
            

            //Calculate Cost

            if (hasReviews == true)
            {
                if (reviewsPage.CssExists(".pagination__list"))
                {
                    var nodeNum = reviewsPage.QuerySelectorAll(".pagination__list > li").Length - 2;
                    reviewsPagesCnt = Convert.ToInt32(reviewsPage.QuerySelectorAll(".pagination__list > li")[nodeNum].QuerySelector("a").InnerText);
                }
                else { reviewsPagesCnt = 1; }
            }
            else { reviewsPagesCnt = 0; }

            if (hasComments == true)
            {
                if (commentsPage.CssExists(".pagination__list"))
                {
                    var nodeNum = commentsPage.QuerySelectorAll(".pagination__list > li").Length - 2;
                    commentsPagesCnt = Convert.ToInt32(commentsPage.QuerySelectorAll(".pagination__list > li")[nodeNum].InnerText.Trim());
                }
                else { commentsPagesCnt = 1; }
            }
            else { commentsPagesCnt = 0; }

            GETreq_fullcost = reviewsPagesCnt + commentsPagesCnt + 1; //Add 1 for the details page

            Debug.WriteLine("HasReviews: " + hasReviews);
            Debug.WriteLine("HasComments: " + hasComments);
            Debug.WriteLine("Reviews Pages Count: " + reviewsPagesCnt);
            Debug.WriteLine("Comments Pages Count: " + commentsPagesCnt);

        }
        public void FullScrapeNewProduct(string ProductURL)
        {
            //Setup and Calculate whcih URLs to add
            
            URL = ProductURL;
            Calculate_GETreq_cost(ProductURL);
            Console.WriteLine("////////////// Calculated Get Request Cost //////////////");
         
            var scraper = new Scraper();
            scraper.ObeyRobotsDotTxt = false;
                       
            //Scrape Item Details Page
            scraper.Request(ProductURL , scraper.Parse); //Product Page
            scraper.Start();
           
                //1:1 and Files
                SetValues(scraper.response_t);
                scraper.DownloadImage(logoURL, ".");

                //1:1_Daily 
                productdetails.SetValues(scraper.response_t, ProductID);
                productStats.SetValues(scraper.response_t);
        
                //1:M
                SetTags(scraper.response_t, ProductID);
                SetAttributes(scraper.response_t);
                SetLicenseValues(scraper.response_t.QuerySelector("#purchase-form > form > div > div:nth-child(4)"));

            //Scrape Comments & Reviews Pages
            if (hasReviews == true)
            {
                Construct_reviewsbaseURL();
                for (int page = 1; page <= reviewsPagesCnt; page++)
                {
                    scraper.Request(reviewsbaseURL + "?page=" + page, scraper.ReviewsPageScrape);
                }
            }

            if (hasComments == true)
            {
                Construct_commentsbaseURL();
                for (int page = 1; page <= commentsPagesCnt; page++)
                {
                    scraper.Request(commentsbaseURL + "?page=" + page, scraper.CommentsPageScrape);
                }
            }

            scraper.Start();
                
            //Copy scraper objects to product object (Unnecessary, only for clarity)
                Reviews = scraper.Reviews;
                Comments = scraper.Comments;

            Debug.WriteLine("Scraped Full Product, Next Step : Write to DB");

            WritetoDB();

        }      
        private void AddProducttoDB(Product product)
        {
            using (var db = new WebScrapeDbContext())
            {
                db.Product.Attach(product);
            }
        }

        public void WritetoDB()
        {
            using (var db = new WebScrapeDbContext())
            {

                //1:1_Daily
                db.Product_StatsViaProfilePage.Add(productStats);

                //1:M
                db.Product_Review.AddRange(Reviews);
                db.Product_Comment.AddRange(Comments);
                db.Product_License.AddRange(licenses);
                db.Product_Tag.AddRange(Tags);

                //These have ChangesetIDs                
                db.Product_DetailsChangeSets.Add(productdetails);
                db.Product_Attribute.AddRange(productAttributes);

                db.SaveChanges();
            }
        }

        public void SetValuesViaList(HtmlNode node, string AuthorName)
        {
            this.Author = AuthorName;
            this.ProductID = node.GetAttribute("data-item-id");
            this.Title = node.QuerySelector(".product-grid__heading > a").InnerText.Trim();
            this.Hierarchy = node.QuerySelector(".js-google-analytics__list-event-trigger > img").GetAttribute("item-category");

        }
        /// <summary>
        /// Key Sequence Method
        /// </summary>
        /// <param name="response"></param>
        public void UpdateTheManyTables (Response response)
        {
            var productstats = new ProductStatsViaPP();
            productstats.SetValues(response);
            
        }


        public int getPreviousCount(string source, string field)
        {
            var count = new int();

            if (this.Author is null == false)
            {
                try
                {
                    //Search for field
                    return count;
                }

                catch (Exception ex)
                {
                    LogWriter logWriter = new LogWriter(" " + this.Author + " not found in DB ::" + ex.Message, "log_updateAuthor");
                    return -1;
                }
            }
            else
            {
                Console.WriteLine("Author is null, cannot retrieve value");
                return -1;
            }
           
        }
        public int determineItemCounttoScrape(int previousItemsCount, int newItemsCount)
        {
            return newItemsCount - previousItemsCount;
            //Reviews and Comments pages contain 20 elements each
            //but Reviews is sorted asc (newest on 1st page) whereas Comments is sorted desc (newest on last page)
        }

        public void ScrapeNewReviews(Response response, int newItemsCount)
        {

            var reviewsNode = response.QuerySelector(".content-s").ChildNodes[3];

            foreach (var node in reviewsNode.ChildNodes)
            {
                var review = new Review();               
                //review.SetValues(node, this.Link);
                               
            }
        }
        public void ScrapeNewComments(Response response, int newItemsCount)
        {

        }

        public static string ReturnIDfromURL(string URL)
        {
            return URL.Split('/')[URL.Split('/').Length - 1];
        }

        public void ApplyProductIDtoAllObjectLists()
        {
            var ID = this.ProductID;

            licenses = licenses.Select(e =>{var ret = e;e.ProductID = ID; return e;}).ToList<License>();
            Tags = Tags.Select(e => { var ret = e; e.ProductID = ID; return e; }).ToList<Tag>();
            productAttributes = productAttributes.Select(e => { var ret = e; e.ProductID = ID; return e; }).ToList<ProductAttribute>();
            Comments = Comments.Select(e => { var ret = e; e.ProductID = ID; return e; }).ToList<Comment>();
            Reviews = Reviews.Select(e => { var ret = e; e.ProductID = ID; return e; }).ToList<Review>();

        }

        
    }


    







    
    }

    
    

