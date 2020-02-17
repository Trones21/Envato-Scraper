using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IronWebScraper;


namespace MyWebScraper
{
    [Table("Authors", Schema = "Author")]
    public class Author
    {
        //Base Info Only Needs to be Scraped when a new author is found (once they make a sale and are added to top Authors list)
        //This will contain some (or all) of the info necessary for the workbook
        //I am under the impression that I should make boolean (has linked SocialProfile -- this seems much easier
        //than doing a bunch of joins for views

        [Key]
        public string AuthorName { get; set; }  //This should be unique b/c it is a part of the URL
        public string MemberSince { get; set; }
        public string Country { get; set; }
        public string EmailAddress { get; set; }
        [Column(TypeName = "Date")]
        public DateTime dateAdded { get; set; }
        public DateTime? datetimeLinkClicked { get; set; }
        /// <summary>
        /// Did they first click via Email? LinkedIn? Facebook? etc.
        /// </summary>
        public string SourceofLinkClicked { get; set; }
        public string password { get; set; }
        public string logoURL { get; set; }
        public List<string> LinksInProfile;
        public List<Product> Products;

        public void FullScrapeNewAuthorProfile(string AuthorName)
        {
            var URL = "https://videohive.net/user/" + AuthorName;
            var scraper = new Scraper();

            //(1:1) Entities
            scraper.Request(URL, scraper.Parse);           
            scraper.Start();
            this.SetValues(scraper.response_t, AuthorName);
            this.UpdateAuthorTable();
            scraper.DownloadImage(this.logoURL, ".");

            var EnvatoSites = new AuthorEnvatoSites();
            EnvatoSites.SetValues(scraper.response_t);
            EnvatoSites.UpdateEnvatoSitesTable(AuthorName);

            var SocialSites = new AuthorSocialSites();
            SocialSites.SetValues(scraper.response_t);
            SocialSites.UpdateSocialNetworksTable(AuthorName);

            //(1:M) Entities
            var StatsviaPP = new AuthorStatsViaPP();
            StatsviaPP.SetValuesViaProfilePage(scraper.response_t);
            StatsviaPP.WritetoDB();
           
            var badgeNodes = scraper.response_t.QuerySelector(".badges").GetElementsByTagName("li");
            var badges = new List<AuthorBadge>();
            foreach (var node in badgeNodes)
            {
                var badge = new AuthorBadge();
                //node.
                //badges.Add();
            }
            AuthorBadge.UpdateDB(badges);

            //////////////////////////////////////

            //Loop through Portfolio for each Site
            var PortfolioSites = AuthorEnvatoSites.GetEnvatoSitesfromDB(AuthorName);
            
            //AddURLS
            foreach (var site in PortfolioSites)
            {
                var baseURL = "https://" + site + ".net/user/" + AuthorName + "/portfolio";
                scraper.Request(baseURL, scraper.Parse); //Automatically sorts by date desc
                scraper.Start();

                if (!scraper.response_t.CssExists(".pagination__list"))
                {
                    scraper.Request(baseURL, scraper.PortfolioPageScrape);
                }
                else
                {
                    var paginationtxt = scraper.response_t.QuerySelector(".pagination__summary").InnerText;
                    int lastPage = Convert.ToInt32(Regex.Match(paginationtxt, "of [0-9]+").Value.Replace("of ", "").Trim());
                    for (int page = 1; page <= lastPage; page++)
                    {
                        scraper.Request(baseURL + "?page=" + page, scraper.PortfolioPageScrape);
                    }
                }                
                Debug.WriteLine(AuthorName + "  " + site + " URLs Added to Queue -- Debug");
            }
            
            //Scrape
            scraper.Start();
            Debug.WriteLine("Finsihed with Author Portfolio Scrape (Method: Product.SetValuesViaList)");
            
            //Write Products to DB 
            using (var db = new WebScrapeDbContext())
            {
                db.Product.AddRange(scraper.Products);
                db.SaveChanges();
            }


            //FullScrape for each Product -- PK_Unfinished
            //var products = GetProducts(this.AuthorName);
            //foreach (var productName in products)
            {
                var product = new Product();
               // product.FullScrapeNewProduct();
            }



        }

        /// <summary>
        /// Adds a new Author to the Author table and all 1:1 tables
        /// </summary>
        public static void AddNewAuthor(string AuthorName)
        {

            using (var context = new WebScrapeDbContext())
            {
                try
                {
                    var pw = CreatePassword(12);
                    var sql = "EXEC dbo.AddAuthor @AuthorName , @pw";
                    context.Database
                        .ExecuteSqlCommand(sql,
                        new SqlParameter("@AuthorName", AuthorName),
                        new SqlParameter("@pw", pw)
                        );

                }
                catch(Exception ex)
                {
                    var failLog = new LogWriter(AuthorName + ex.Message, "AddAuthorFails");
                    Console.WriteLine(AuthorName + " failed to write to DB");
                }
            }

        }
        private static string CreatePassword(int length)
        {

            string password = Guid.NewGuid().ToString("N").ToLower()
                      .Replace("1", "").Replace("o", "").Replace("0", "")
                      .Substring(0, length);
            return password;

        }

        private List<string> GetProducts(string AuthorName)
        {
            //PK_Unfinished
            using (var db = new WebScrapeDbContext())
            {

                return new List<string>();
            }
              
        }

        /// <summary>
        /// Sets the values for the Author Class
        /// </summary>
        private void SetValues(Response response, string AuthorName)
        {
            this.AuthorName = AuthorName;
            //this.Country = response.QuerySelector("");
            this.MemberSince = Regex.Match(response.QuerySelector(".user-info-header__content > p").InnerText, "since \\w+ \\w+")
                                        .Value.Replace("since", "").Trim();

            //this.logoURL = ;
        }

        private void UpdateAuthorTable()
        {
            using (var db = new WebScrapeDbContext())
            {
                var author = db.Author.Single(a => a.AuthorName == this.AuthorName);
                if (this.Country != null) { author.Country = this.Country; }
                if (this.logoURL != null) { author.logoURL = this.logoURL; }
                if (this.MemberSince != null) { author.MemberSince = this.MemberSince; }

                db.SaveChanges();
            }
        }
     
    }

    [Table("StatsViaList", Schema = "Author")]
    public class AuthorStatsViaList
    {
        [Key, Column(Order = 0, TypeName = "Date")]
        public DateTime Date { get; set; }
        [Key, Column(Order = 1)]
        public string AuthorName { get; set; }
        public int OverallRank { get; set; }
        public double? Rating { get; set; }
        public int RatingsCount{ get; set; }
        public int SalesCount { get; set; }
        public int followersCount { get; set; }        
        public int ItemsCount { get; set; }
        public bool IsAvailableforFreelance { get; set; }
        public int BadgesCount { get; set; }

        public List<string> ProfileBadgesText = new List<string>();

        public void SetValuesViaList(HtmlNode htmlNode)
        {
            //Elements will always exist
            var text = htmlNode.QuerySelector("div.user-info > h3 > a").TextContentClean;
            AuthorName = text.Split(' ')[1].Trim();
            Date = DateTime.Now;
            
            this.OverallRank = Convert.ToInt32(text.Split('.')[0].Replace(".", "").Trim());
            this.BadgesCount = htmlNode.QuerySelectorAll("div.user-info > div > ul > li").Count();
           
            try
            {
                this.SalesCount = Convert.ToInt32(htmlNode.QuerySelector("div.sale-info.is-hidden-phone > em").InnerTextClean.Replace(",", ""));
            }
            catch { }

            //Must check that these elements exist
            var temp = htmlNode.QuerySelector(".meta.is-hidden-phone").InnerText.Split(':')[1];
            if (temp.Contains("Available")) { this.IsAvailableforFreelance = true; } else { this.IsAvailableforFreelance = false; }

            var test4substrings = htmlNode.QuerySelector("small.meta.is-hidden-phone").InnerText;
            if (test4substrings.Contains("Items") & test4substrings.Contains("Followers"))
            {
                this.ItemsCount = Convert.ToInt32(htmlNode.QuerySelector("small.meta.is-hidden-phone > strong:nth-child(1)").InnerTextClean.Replace(",", ""));
                this.followersCount = Convert.ToInt32(htmlNode.QuerySelector("small.meta.is-hidden-phone > strong:nth-child(1)").InnerTextClean.Replace(",", ""));
            }
            else
            {
                if (test4substrings.Contains("Items"))
                {
                    this.ItemsCount = Convert.ToInt32(htmlNode.QuerySelector("small.meta.is-hidden-phone > strong:nth-child(1)").InnerTextClean.Replace(",", ""));
                }

                if (test4substrings.Contains("Followers"))
                {
                    this.followersCount = Convert.ToInt32(htmlNode.QuerySelector("small.meta.is-hidden-phone > strong:nth-child(1)").InnerTextClean.Replace(",", ""));
                }
            }

            if (htmlNode.QuerySelector(".rating-basic__count") == null)
            { this.RatingsCount = 0; }
            else
            {
                var ratingsraw = htmlNode.QuerySelector(".rating-basic__count").InnerTextClean;
                ratingsraw = ratingsraw.Split(' ')[0].Replace(",", "").Trim();
                this.RatingsCount = Convert.ToInt32(ratingsraw);

                var stars = htmlNode.QuerySelector(".rating-basic__stars").GetElementsByTagName("img");
                var tempRating = new double();
                foreach (var star in stars)
                {
                    if (Regex.IsMatch(star.GetAttribute("src"), "half"))
                    {
                        tempRating += .5;
                    }
                    else if (Regex.IsMatch(star.GetAttribute("src"), "empty"))
                    { }
                    else
                    {
                        tempRating += 1;
                    }                      
                }
                this.Rating = tempRating; 
            }

        }
    }

    [Table("StatsViaProfilePage", Schema = "Author")]
    public class AuthorStatsViaPP
    {
        [Key, Column(Order = 0, TypeName = "Date")]
        public DateTime Date { get; set; }
        [Key, Column(Order = 1)]
        public string AuthorName { get; set; }
        public int RatingsCount { get; set; }
        public decimal RatingPrecise { get; set; }
        public int SalesCount { get; set; }
        public int followersCount { get; set; }
        public int followingCount { get; set; }
        public int ItemsCount { get; set; }
        public bool IsAvailableforFreelance { get; set; }
        public int BadgesCount { get; set; }      

        public void SetValuesViaProfilePage(Response response)
        {
            Date = DateTime.Now;
            AuthorName = response.QuerySelector(".t-heading.h-display-inlineblock.h-m0.h-p0.-size-m").InnerText.Trim();

            RatingsCount = Convert.ToInt32(Regex.Match(response.QuerySelector(".user-info-header__stats-content.-extra-padding > span").InnerText, "[0-9]+").Value);
            this.RatingPrecise = Convert.ToDecimal(response.QuerySelector(".star-rating > span").InnerText.Replace("stars", "").Trim());
            this.SalesCount = Convert.ToInt32(response.QuerySelectorAll(".user-info-header__stats-content")[1].InnerText.Replace(",","").Trim());

            var nodes = response.QuerySelectorAll(".site-portfolio");
            var tempitemsCount = 0;
            foreach (var node in nodes)
            {
                tempitemsCount += Convert.ToInt32(node.QuerySelector("div > span").InnerText.Trim());
            }
            this.ItemsCount = tempitemsCount;

            this.IsAvailableforFreelance = response.CssExists(".country-info__available");

            var tabs = response.QuerySelector(".page-tabs").GetElementsByTagName("li");
            foreach (var tab in tabs)
            {
                switch (tab)
                {
                    case var match when (tab.QuerySelector("a").InnerText.Contains("Followers")):
                        this.followersCount = Convert.ToInt32(tab.QuerySelector("span").InnerText.Trim());
                        break;
                    case var match when (tab.QuerySelector("a").InnerText.Contains("Following")):
                        this.followingCount = Convert.ToInt32(tab.QuerySelector("span").InnerText.Trim());
                        break;
                    default: break;
                }
            }
            
            this.BadgesCount = response.QuerySelector(".badges").GetElementsByTagName("li").Length;
        }

        internal void WritetoDB()
        {
            using (var db = new WebScrapeDbContext())
            {
                db.Author_StatsViaProfilePage.Add(this);
            }
        }
    }

    [Table("SocialSites", Schema = "Author")]
    public class AuthorSocialSites
    {
        [Key]
        public string AuthorName { get; set; }       
        public string Behance { get; set; }
        public string DeviantArt { get; set; }
        public string Digg { get; set; }
        public string Dribbble { get; set; }
        public string Facebook { get; set; }
        public string Flickr { get; set; }
        public string Github { get; set; }
        public string GooglePlus { get; set; }
        public string LastFM { get; set; }
        public string LinkedIn { get; set; }
        public string MySpace { get; set; }
        public string Reddit { get; set; }
        public string SoundCloud { get; set; }
        public string Tumblr { get; set; }
        public string Twitter { get; set; }
        public string Vimeo { get; set; }
        public string Youtube { get; set; }

        public void SetValues(Response response)
        {
            if (response.CssExists(".social-networks") == true)
            {
                var socialNetworks = response.Css(".social-networks");
                foreach (var node in socialNetworks[0].ChildNodes)
                {
                    if (node.NodeName == "A")
                    {
                        var link = node.GetAttribute("href").ToString();                        
                        switch (link)
                        {                           
                            case var match when (link.Contains("behance.net")): this.Behance = link; break;                            
                            case var match when (link.Contains("deviantart.com")): this.DeviantArt = link; break;
                            case var match when (link.Contains("dribbble.com")): this.Dribbble = link; break;
                            case var match when (link.Contains("digg.com")): this.Digg = link; break;
                            case var match when (link.Contains("facebook.com")): this.Facebook = link; break;
                            case var match when (link.Contains("flickr.com")): this.Flickr = link; break;
                            case var match when (link.Contains("github.com")): this.Github = link; break;
                            case var match when (link.Contains("plus.google.com")): this.GooglePlus = link; break;
                            case var match when (link.Contains("linkedin.com")): this.LinkedIn = link; break;
                            case var match when (link.Contains("last.fm")): this.LastFM = link; break;
                            case var match when (link.Contains("myspace.com")): this.MySpace = link; break;
                            case var match when (link.Contains("reddit.com")): this.Reddit = link; break;
                            case var match when (link.Contains("soundcloud.com")): this.SoundCloud = link; break;
                            case var match when (link.Contains("tumblr.com")): this.Tumblr = link; break;
                            case var match when (link.Contains("twitter.com")): this.Twitter = link; break;
                            case var match when (link.Contains("vimeo.com")): this.Vimeo = link; break;
                            case var match when (link.Contains("youtube.com")): this.Youtube = link; break;
                        }
                    }
                }
            }
     
        }

        public void UpdateSocialNetworksTable(string AuthorName)
        {
                using (var db = new WebScrapeDbContext())
                {
                var SocialSites = db.Author_SocialSites.Single(s => s.AuthorName == AuthorName);
                if (this.Behance != null)       { SocialSites.Behance = this.Behance; }
                if (this.DeviantArt != null)    { SocialSites.DeviantArt = this.DeviantArt; }
                if (this.Digg != null)          { SocialSites.Digg = this.Digg; }
                if (this.Dribbble != null)      { SocialSites.Dribbble = this.Dribbble ; }
                if (this.Facebook != null)      { SocialSites.Facebook = this.Facebook ; }
                if (this.Flickr != null)        { SocialSites.Flickr = this.Flickr ; }
                if (this.Github != null)        { SocialSites.Github = this.Github ; }
                if (this.GooglePlus != null)    { SocialSites.GooglePlus = this.GooglePlus ; }
                if (this.LastFM != null)        { SocialSites.LastFM = this.LastFM ; }
                if (this.LinkedIn != null)      { SocialSites.LinkedIn = this.LinkedIn ; }
                if (this.MySpace != null)       { SocialSites.MySpace = this.MySpace ; }
                if (this.Reddit != null)        { SocialSites.Reddit = this.Reddit ; }
                if (this.SoundCloud != null)    { SocialSites.SoundCloud = this.SoundCloud ; }
                if (this.Tumblr != null)        { SocialSites.Tumblr = this.Tumblr ; }
                if (this.Twitter != null)       { SocialSites.Twitter = this.Twitter ; }
                if (this.Vimeo != null)         { SocialSites.Vimeo = this.Vimeo ; }
                if (this.Youtube != null)       { SocialSites.Youtube = this.Youtube ; }

                db.SaveChanges();
            }
                
 

        }
    }

    [Table("EnvatoSites", Schema = "Author")]
    public class AuthorEnvatoSites
    {
        //Use these to figure out which domain to scrape (So we can see the portfolio) -- or can I scrape the portfolio from any domain i.e domain/user/portfolio???        
        [Key]
        public string AuthorName { get; set; }
        public bool IsOnAudioJungle { get; set; }
        public DateTime? DateJoinedAudioJungle { get; set; }
        public bool IsOnCodeCanyon { get; set; }
        public DateTime? DateJoinedCodeCanyon { get; set; }
        public bool IsOnGraphicRiver { get; set; }
        public DateTime? DateJoinedGraphicRiver { get; set; }
        public bool IsOnPhotoDune { get; set; }
        public DateTime? DateJoinedPhotoDune { get; set; }
        public bool IsOnThemeForest { get; set; }
        public DateTime? DateJoinedThemeForest { get; set; }
        public bool IsOnVideoHive { get; set; }
        public DateTime? DateJoinedVideoHive { get; set; }
        public bool IsOn3DOcean { get; set; }
        public DateTime? DateJoined3DOcean { get; set; }


        public void SetValues(Response response)
        {
            var sites = response.Css(".site-portfolio");
            foreach (var site in sites)
            {
                var text = site.GetElementsByTagName("a")[0].InnerText;
                switch (text.ToLower())
                {
                    //Set the DateTime when comparing to the database
                    case "audiojungle items":   this.IsOnAudioJungle = true; break;
                    case "codecanyon items":    this.IsOnCodeCanyon = true; break;
                    case "graphicriver items":  this.IsOnGraphicRiver = true; break;
                    case "photodune items":     this.IsOnPhotoDune = true; break;
                    case "themeforest items":   this.IsOnThemeForest = true; break;
                    case "videohive items":     this.IsOnVideoHive = true; break;
                    case "3docean items":       this.IsOn3DOcean = true; break;
                }
            }
        }

        public void UpdateEnvatoSitesTable(string AuthorName)
        {
                using (var db = new WebScrapeDbContext())
                {
                var envatoSites = db.Author_EnvatoSites.Single(e => e.AuthorName == AuthorName);

                envatoSites.IsOn3DOcean = this.IsOn3DOcean;
                envatoSites.IsOnAudioJungle = this.IsOnAudioJungle;
                envatoSites.IsOnCodeCanyon = this.IsOnCodeCanyon;
                envatoSites.IsOnGraphicRiver = this.IsOnGraphicRiver;
                envatoSites.IsOnPhotoDune = this.IsOnPhotoDune;
                envatoSites.IsOnThemeForest = this.IsOnThemeForest;
                envatoSites.IsOnVideoHive = this.IsOnVideoHive;

                //PK_Unfinished
                //envatoSites.Date

                db.SaveChanges();
                }
            }
        public static List<string> GetEnvatoSitesfromDB(string AuthorName)
        {

            var PortfolioSites = new List<string>();
            using (var db = new WebScrapeDbContext())
            {
                var EnvatoSitesobj = db.Author_EnvatoSites.Single(s => s.AuthorName == AuthorName);
                if(EnvatoSitesobj.IsOn3DOcean == true) { PortfolioSites.Add("3DOcean"); }
                if (EnvatoSitesobj.IsOnAudioJungle == true) { PortfolioSites.Add("AudioJungle"); }
                if (EnvatoSitesobj.IsOnCodeCanyon == true) { PortfolioSites.Add("CodeCanyon"); }
                if (EnvatoSitesobj.IsOnGraphicRiver == true) { PortfolioSites.Add("GraphicRiver"); }
                if (EnvatoSitesobj.IsOnPhotoDune == true) { PortfolioSites.Add("PhotoDune"); }
                if (EnvatoSitesobj.IsOnThemeForest == true) { PortfolioSites.Add("ThemeForest"); }
                if (EnvatoSitesobj.IsOnVideoHive == true) { PortfolioSites.Add("VideoHive"); }
            }
                return PortfolioSites;
            }
        }
       

    [Table("Badges", Schema = "Author")]
    public class AuthorBadge
    {
        // Authorname cannot be key -- must be a composite key AuthorBadge
        [Key]
        public string AuthorName_BadgeName { get; set; }
        public DateTime DateAquired { get; set; }
        public string BadgeName { get; set; }

     /// <summary>
        /// Adds AuthorName_BadgeName from List that do not already exist in Table
        /// </summary>
        /// <param name="badges"></param>
        /// <returns></returns>
    public static bool UpdateDB(List<AuthorBadge> badges)
        {
            using (var db = new WebScrapeDbContext())
            {
                //PK_Unfinished
                //db.
                //db.DbSet_Authorbadge.AddRange(badges)
                db.SaveChanges();
                return true;
            }

        }
    }

}
