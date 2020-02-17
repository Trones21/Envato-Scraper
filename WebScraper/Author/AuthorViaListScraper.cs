using IronWebScraper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NS_EmailSender;

namespace MyWebScraper
{
    public class AuthorViaListScraper : WebScraper
    {
        public int totalPages = new int();
        public List<AuthorStatsViaList> authorsStatsViaList = new List<AuthorStatsViaList>();
        public Dictionary<string, string> masterbadgesList = new Dictionary<string, string>();
        public Counter counter = new Counter();
        public List<string> NewAuthors = new List<string>();

        public override void Init()
        {
            this.Identities = Program.HttpIdentities();
            this.LoggingLevel = WebScraper.LogLevel.Critical;
            this.RateLimitPerHost = TimeSpan.FromMilliseconds(200);
            counter.count = 0;
        }
        public void AddURLs()
        {
            for (int pageNum = 1; pageNum <= totalPages; pageNum++)
            {
                
              this.Request("https://videohive.net/authors/top?level=all&page=" + pageNum + "&site=all", Parse);
               
            }           
        }

        public override void Parse(Response response)
        {

            Console.WriteLine(DateTime.Now.ToLongTimeString() +" : URL Begin : " + response.FinalUrl);
            var listlength = 50; //
            for (int ElementNum = 1; ElementNum <= listlength; ElementNum++) //Looping through 
            {
                var authorStats = new AuthorStatsViaList();
                var htmlnode = response.QuerySelectorAll(".user-list > li")[ElementNum - 1];
                authorStats.SetValuesViaList(htmlnode);
                authorsStatsViaList.Add(authorStats);
            }

            counter.increment();
            if (counter.count == 24)
            {
                Console.WriteLine("25 Urls scraped  :  " + DateTime.Now.ToLongTimeString());
                counter.count = 0;
            }
        }
        
        public void WritetoAuthorStatsTable()
        {
            try
            {
                using (var context = new WebScrapeDbContext())
                {
                    context.Author_StatsViaList.AddRange(this.authorsStatsViaList);
                    context.SaveChanges();
                }
                Debug.WriteLine("Wrote to AuthorStats Table");
            }
            catch (Exception ex)
            {
                var dblog = new LogWriter(ex.Message, "db_log");
                Debug.WriteLine("Failed writing to AuthorStats Table");                
            }
        }
        public void NewAuthorsSequence()
        {
            //Sequence is important -- Everything must be queried and updated in a certain order
            GetNewAuthorsForCongratsWorkbook();

            foreach (var author in this.NewAuthors)
            {
                Author.AddNewAuthor(author);
            }

            //foreach (var author in this.NewAuthors)
            //{
            //    //Method For a Full author get -- So get their profile, products... everything
            //    var authorObj = new Author();
            //    authorObj.FullScrapeNewAuthorProfile(author);
            //}

            //foreach (var author in this.NewAuthors)
            //{
            //    CreateNewAuthorTableauWorkbook(author);
            //}
            
            ////Not sure of this part of the sequence yet... this could involve many more methods, preparing html etc.
            //SendtoAuthor();
        }
        private void GetNewAuthorsForCongratsWorkbook()
        {

            using (var context = new WebScrapeDbContext())
            {
                var newauthors = context.Database.SqlQuery<string>(@"Select Distinct ad.AuthorName from Author.StatsViaList ad
                                                     left join Author.Authors a on a.AuthorName = ad.AuthorName
                                                     where a.AuthorName is null;");

                this.NewAuthors = newauthors.ToList();
            }
        }
        
        private void CreateNewAuthorTableauWorkbook(string AuthorName)
            {
                //throw new NotImplementedException();
            }               
        private void SendtoAuthor(Author author)
            {
            //Might do this with UiPath
            //Idea - I want to distribute this to as many social channels as possible
            //This will be a webpage with a link to download Tableau workbook "for more indepth competitive intelligence"
            //URL: https://mywebsite.com/NewTopAuthor?u=<AuthorName>&s=<wherethelinkisSentto>&uid=<password>
            //throw new NotImplementedException();
            
            var CongratsEmail = new EmailSender();
            //CongratsEmail.ConstructHTML("PK_enum", author);
            CongratsEmail.SendMail(true, "Congrats " + author.AuthorName + "! You have made your first sale on the Envato Market" );
            }
        
        #region old
        private void depc_AddAllNewAuthors()
        {
            //I should change this to generate a random password and add it
            using (var context = new WebScrapeDbContext())
            {
                //Write a SP for this. 
                var addAuthortoAuthorTable = context.Database
                                .ExecuteSqlCommand(@"Insert into Authors (AuthorName)
                                                         Select Distinct ad.AuthorName from dbo.AuthorStatsViaLists ad
                                                         left join Authors a on a.AuthorName = ad.AuthorName
                                                         where a.AuthorName is null;");

                var addAuthortoAuthorEnvatoSites = context.Database
                                .ExecuteSqlCommand(@"Insert into AuthorEnvatoSites (AuthorName)
                                                         Select Distinct ad.AuthorName from dbo.AuthorStatsViaLists ad
                                                         left join Authors a on a.AuthorName = ad.AuthorName
                                                         where a.AuthorName is null;");

                var addAuthortoAuthorSocialSites = context.Database
                                .ExecuteSqlCommand(@"Insert into AuthorSocialSites (AuthorName)
                                                         Select Distinct ad.AuthorName from dbo.AuthorStatsViaLists ad
                                                         left join Authors a on a.AuthorName = ad.AuthorName
                                                         where a.AuthorName is null;");

            }

        }
        public void ListofBadgestoCSV()
        {

            using (TextWriter tw = File.CreateText("CSV\\Badges" + DateTime.Now.ToString("yyyyMMdd") + ".csv"))
            {
                foreach (var badge in masterbadgesList)
                {
                    tw.WriteLine(badge.Key + "," + badge.Value);
                }
            }
        }
        public void WritetoCSV()
        {

            using (TextWriter tw = File.CreateText("CSV\\AuthorTest_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv"))
            {
                foreach (var line in UnsortedMethods.ToCsv(",", authorsStatsViaList))
                {
                    tw.Write(DateTime.Now.ToShortDateString() + ",");
                    tw.WriteLine(line);
                }
            }

            using (TextWriter tw = File.AppendText("CSV\\BadgesDaily.csv"))
            {
                foreach (var author in this.authorsStatsViaList)
                {
                    foreach (var badge in author.ProfileBadgesText)
                    {
                        tw.WriteLine(author.Date + author.AuthorName + "," + badge);
                    }
                }
            }
        }
        #endregion

    }
}

