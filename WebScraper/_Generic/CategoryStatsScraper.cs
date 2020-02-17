using IronWebScraper;
using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace MyWebScraper
{
    [Table("Stats", Schema = "ProductCategory")]
    public class ProductCategoryStats
    {
        [Key]
        public string date_URL { get; set; }
        [Column]
        public DateTime Date { get; set; }
        public int Level { get; set; }
        public string URL { get; set; }
        public string Category { get; set; }
        public int ItemsCount { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public virtual ICollection<Dictionary<string, int>> Tags { get; set; } //Name and Count
        public virtual List<SiteFilter> Filters { get; set; }  

        /// <summary>
        /// This depends on the site
        /// </summary>
        /// <param name="filterspanel"></param>
        /// <param name="URL"></param>
        public void SetValues(HtmlNode filterspanel, string URL)
        {
            this.Category = filterspanel.QuerySelector("._1OYwR._1HFue").InnerText;
            this.Date = DateTime.Today;
            this.URL = URL;
            this.Level = URL.Split(new[] { ".net" }, StringSplitOptions.None)[1].Split('/').Length; //Count the Slashes
            var itemsRaw = filterspanel.QuerySelector("._1OYwR._1HFue").ParentNode.QuerySelector("span").InnerText;
            this.ItemsCount = Convert.ToInt32(itemsRaw.Replace(",", ""));
            
            this.MinPrice = Convert.ToInt32(filterspanel.QuerySelector("div[data-test-selector=filter-price] > div > div > div:nth-child(1) > input").GetAttribute("placeholder"));
            this.MaxPrice = Convert.ToInt32(filterspanel.QuerySelector("div[data-test-selector=filter-price] > div > div > div:nth-child(3) > input").GetAttribute("placeholder"));

            //this.Tags = CreateDictionary(filterspanel, "tags", "._checkboxText_1n0id_69", "._2kkyX");



            //this.Sales = CreateDictionary(filterspanel, "sales", "._checkboxText_1n0id_69", "._2kkyX");
            //this.Rating = CreateDictionary(filterspanel, "rating", "span", "._3RQES");
            //this.DateAdded = CreateDictionary(filterspanel, "date-added", "span", "._3RQES");

            Console.WriteLine("Scraped" + this.URL);
        }
        /// <summary>
        /// This is the generalized method that will work for any of the nodes.
        ///The Productcategory class no longer reflects this setup. (Using classes with Explicit names rather than storing these as strings) 
        /// </summary>
        /// <param name="filterspanel"></param>
        /// <param name="queryPathVal"></param>
        /// <param name="keySelector"></param>
        /// <param name="valueselector"></param>
        /// <returns></returns>
        public Dictionary<string, int> CreateDictionary(HtmlNode filterspanel, string queryPathVal, string keySelector, string valueselector)
        {
            var newDict = new Dictionary<string, int>();
            var Nodes = filterspanel.QuerySelector("div[data-test-selector=filter-"+ queryPathVal +"] > div > div").ChildNodes;
            foreach (var node in Nodes)
            {
                var key = node.QuerySelector(keySelector).InnerText.Trim();
                var value = Convert.ToInt32(node.QuerySelector(valueselector).InnerText.Replace(",","").Trim());
                newDict.Add(key, value);
            }
            return newDict;
        }

        public List<string> ReturnTagURLs(HtmlNode filterspanel)
        {
            List<string> TagURLs = new List<string>();
            var tagNodes = filterspanel.QuerySelector("div[data-test-selector=filter-tags] > div > div").ChildNodes;
            foreach (var tag in tagNodes)
            {
                var key = tag.QuerySelector("._checkboxText_1n0id_69").InnerText;

            }

            return TagURLs;
        }

    }

    [Table("SiteFilters", Schema = "ProductCategory")]
    public class SiteFilter
    {
        //PK - date_URL_FilterName_FilterMember
        [Key, Column(Order = 0 , TypeName = "Date")]
        public DateTime date { get; set; }
        [Key, Column(Order = 1)]
        public string URL { get; set; }
        [Key, Column(Order = 2)]
        public string FilterName { get; set; }
        [Key, Column(Order = 3)]
        public string FilterMember { get; set; }
        public string Value { get; set; }

        public void SetValues(HtmlNode filtersPanel)
        {
        //Will need some kind of logic for different selectors, see CreateDictionary for an example
        }

    }



    public class CategoryStatsScraper : WebScraper
    {
        public List<string> TagURLs = new List<string>();
        public List<ProductCategoryStats> prodCats = new List<ProductCategoryStats>();
        public List<string> ScrapedLinks = new List<string>();
        string baseURL;
        

        public override void Init()
        {
            this.Identities = Program.HttpIdentities();
            this.LoggingLevel = LogLevel.Critical;        
        }

        public void NewDomain(string baseURL, string startpath)
        {
            this.baseURL = baseURL;
            this.Request(baseURL + startpath, Parse);
        }

        public override void Parse(Response response)
        {           
            var filterspanel = response.QuerySelector("div[data-test-selector=search-filters]");

            var prodCat = new ProductCategoryStats();
            prodCat.SetValues(filterspanel, response.RequestlUrl);
            prodCats.Add(prodCat);

            this.TagURLs.AddRange(prodCat.ReturnTagURLs(filterspanel));
                    
            CheckForNewURLs(filterspanel);           
            }
        private void CheckForNewURLs(HtmlNode filterspanel)
        {
            //This selects everything in the panel so we keep track of the links scraped to avoid multiple scrape/inifnite loop
            var categories = filterspanel.QuerySelector("ul[data-test-selector=category-filter]").ChildNodes;

            foreach (var category in categories)
            {
                var link = category.QuerySelector("a").GetAttribute("href");
                if (!ScrapedLinks.Contains(link))
                {
                    ScrapedLinks.Add(link);
                    this.Request(link, Parse);

                }
                else { Console.WriteLine(link + "has already been added to scrape list (This is normal behavior)"); }

                if (ScrapedLinks.Count > 20)
                {
                    Console.WriteLine("Test is not more than 20 links");
                }
            }
        }

        public static List<SiteFilter> GetFilters(Response response)
        {

            var filters = new List<SiteFilter>();
            var filterNodes = response.QuerySelectorAll("._1loc3");            
            foreach (var node in filterNodes)
            {               
                var filter  = new SiteFilter();
                filter.FilterName = node.GetAttribute("data-test-selector");
                filter.URL = response.Request.Url;
                filters.Add(filter);
            }
            return filters;
        }

        private void WriteToCSV()

            { using (TextWriter tw = File.CreateText("CategoryStatsScrape_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv"))
                {
                    foreach (var line in UnsortedMethods.ToCsvIgnoreLists(",", this.prodCats))
                    {
                        tw.Write(DateTime.Now.ToShortDateString() + ",");
                        tw.WriteLine(line);
                    }
                }
                }
        public void WritetoDB()
        {
            using (var db = new WebScrapeDbContext())
            {
                db.ProdCatStats.AddRange(prodCats);
                db.SaveChanges();
            }
        }
        }

}

