using IronWebScraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MyWebScraper._Generic;
using GetRandomhttpIdentities;
using NS_EmailSender;


namespace MyWebScraper
{
    public class Program
    {
        
        public static EmailSender SummaryEmailer = new EmailSender();
        static void Main(string[] args)
        {
            
            Directory.CreateDirectory("Resources");
            ScrapeRandomHttpIdentities.Main("Resources\\TopUserAgents.txt");

            IronWebScraper.License.LicenseKey = "IRONWEBSCRAPER-137008-598547-27D30CC7-7B29E45D-E6B1CC56-VALID-2017";
            bool result = IronWebScraper.License.IsValidLicense("IRONWEBSCRAPER-137008-598547-27D30CC7-7B29E45D-E6B1CC56-VALID-2017");
            
            Console.WriteLine("IWS license is: " + result);

            //int pages = 100;
            //WarmCache(pages);
            //SubMainMethod(pages);
            //PostScrapeTasks(pages);
            //ProductCategoriesScrape();
            Sandbox();
            //AuthorswithNewProducts();
            
            //DeleteAllData();
        }

        public static void ProductCategoriesScrape()
        {
            var categoryStatsScraper = new CategoryStatsScraper();
            categoryStatsScraper.NewDomain("https://audiojungle.net/category/", "all#content");
            categoryStatsScraper.Start();
            Console.WriteLine("BP");
            categoryStatsScraper.WritetoDB();
        }

        private static void AuthorswithNewProducts()
        {
            throw new NotImplementedException();
            var authorsWithNewProducts = new Dictionary<string, int>();
            using (var db = new WebScrapeDbContext())
            {
                
                db.Database.ExecuteSqlCommand("EXEC dbo.CheckforNewProducts");
                authorsWithNewProducts = db.Database.SqlQuery<QueryResult>("select * from dbo.SP_Results").ToDictionary(r => r.OutputString, r => r.OutputInt);
               
            }
            foreach (var author in authorsWithNewProducts)
            {
                //if(author.Value > )
            }
        }

        private static void PostScrapeTasks(int pages)
        {
            var db = new WebScrapeDbContext();
            var insertedcnt = ParameterizedQueries.CountInsertedToday(db, "[Author].[StatsViaList]", "[Date]");
            SummaryEmailer.AddText("\n Author Stats Via List" +
                                   "\n Expected " + pages * 50 + " records");
            SummaryEmailer.AddText("\n Inserted " + insertedcnt + " records");
            SummaryEmailer.SendMail("Daily Scrape Complete", SummaryEmailer.SavedMessageBody);
        }

        private static void WarmCache(int pages)
        {
            
            var cacheWarmer = new Scraper();
            for (int page = 1; page <= pages; page++)
            {
                cacheWarmer.Request("https://themeforest.net/authors/top?level=all&site=all&page=" + page, cacheWarmer.ParseWithCounter);
            }
            cacheWarmer.WritelineEach = 25;
            cacheWarmer.Start();

            var AuthorListpgcnt = new AuthorViaListPageCountScraper();
            AuthorListpgcnt.Start();
            var logWriter = new LogWriter("AuthorList PageCount: " + AuthorListpgcnt.totalPages, "log_totalAuthorPages");
            SummaryEmailer.AddText("AuthorList PageCount: " + AuthorListpgcnt.totalPages);
        }

        private static void DeleteAllData()
        {
            using (var context = new WebScrapeDbContext())
            {
                context.Database.Delete();
                context.Database.Create();
            }
        }

        public static void SubMainMethod(int pages)
        {
            var AuthorListpgcnt = new AuthorViaListPageCountScraper();
            AuthorListpgcnt.Start();
            var logWriter = new LogWriter("AuthorList PageCount: " + AuthorListpgcnt.totalPages, "log_totalAuthorPages");
            SummaryEmailer.AddText("AuthorList PageCount: " + AuthorListpgcnt.totalPages);

            var authorviaListScraper = new AuthorViaListScraper();
            authorviaListScraper.totalPages = pages;//AuthorListpgcnt.totalPages;  //Just for testing -- Normally I would use AuthorListpgcnt.totalPages;
            authorviaListScraper.AddURLs();
            authorviaListScraper.Start();
         
            authorviaListScraper.WritetoAuthorStatsTable();
            //authorviaListScraper.NewAuthorsSequence();

            logWriter.LogWrite("FailedURLCount:     " + authorviaListScraper.FailedUrls, "log_runstats", LogWriter.format.SingleLine);
            logWriter.LogWrite("SuccessfulURLCount: " + authorviaListScraper.SuccessfulfulRequestCount, "log_runstats", LogWriter.format.SingleLine);
          
            //var authorviaPPScraper = new AuthorViaProfilePageScraper();
            //scraper.Start();

            //foreach (var author in scraper.Authors)
            //{
            //    Console.WriteLine(author);
            //}

            //var categoryStatsScraper = new CategoryStatsScraper();
            //categoryStatsScraper.NewDomain("https://graphicriver.net/", "all-graphics#content");
            //categoryStatsScraper.Start();


            //var productListScraper = new ProductViaListScraper();
            //var productscraper = new ProductScraper();

            //var collectionspgcnt = new CollectionsPageCountScraper();
            //var collectionsScraper = new CollectionScraper();

            //SummaryEmailer.SendMail("Summary for: " + DateTime.Now, SummaryEmailer.SavedMessageBody);
        }

       
        static void FullAuthorScrapeAndWriteAll()
        {


            //var authorviaPPScraper = 
            //scraper.Start();

            //foreach (var author in scraper.Authors)
            //{
            //    Console.WriteLine(author);
            //}

            //var categoryStatsScraper = new CategoryStatsScraper();
            //categoryStatsScraper.NewDomain("https://graphicriver.net/", "all-graphics#content");
            //categoryStatsScraper.Start();


            //var productListScraper = new ProductViaListScraper();
            //var productscraper = new ProductScraper();

            //var collectionspgcnt = new CollectionsPageCountScraper();
            //var collectionsScraper = new CollectionScraper();

            //SummaryEmailer.SendMail("Summary for: " + DateTime.Now, SummaryEmailer.SavedMessageBody);
        }
        /// <summary>
        /// This is for testing specific scrapers. The only issue is that i don't know if a scraper is dependent on another scraper having values in the DB
        /// Idea 1: Add this info to the popup on the scraper
        /// Idea 2: I should create a visio diagram of this.
        /// </summary> 
        public static void Sandbox()
        {
            //var author = new Author();
            //Author.AddNewAuthor("butlerm");
            //author.FullScrapeNewAuthorProfile("butlerm");
            var product = new Product();
            product.FullScrapeNewProduct("https://videohive.net/item/handy-seamless-transitions-pack-script/18967340");
            
            //Create a Tableau workbook to verify it is working
            //but also a TestMethod would be good.
        }

        private static void CheckforFolders()
        {
            if(!Directory.Exists("CSV"))
            {
                Directory.CreateDirectory("CSV");
            }
        }

        public static List<HttpIdentity> HttpIdentities()
        {
            var httpIdentities = new List<HttpIdentity>();


            string[] UserAgents = File.ReadAllLines("Resources\\TopUserAgents.txt");
            foreach (var UserAgent in UserAgents)
            {
                HttpIdentity identity = new HttpIdentity();
                identity.UserAgent = UserAgent;
                httpIdentities.Add(identity);
            }

            return httpIdentities;

        }
      
        }

    }



    
