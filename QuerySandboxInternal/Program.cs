using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWebScraper;

namespace QuerySandboxInternal
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new WebScrapeDbContext())
            {
                try
                {
                    var succeed = context.
                    Database.ExecuteSqlCommand(@"EXEC dbo.AddAuthor 'abcd' , '1234';");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to Insert Author");
                    Console.WriteLine(ex.Message);
                    
                }
            }

            //using (var context = new WebScrapeDbContext())
            //{
            //    var arrange_deleternd = context.
            //        Database.ExecuteSqlCommand(@"delete from Authors
            //                                     where AuthorName like 's%';");
            //}

            //using (var context = new WebScrapeDbContext())
            //{
            //    var newauthors = context.Database.SqlQuery<string>(@"Select Distinct ad.AuthorName from dbo.AuthorStatsViaLists ad
            //                                         left join Authors a on a.AuthorName = ad.AuthorName
            //                                         where a.AuthorName is null;");
            //    foreach (var a in newauthors)
            //    {
            //        Console.WriteLine(a.ToString());
            //    }
            //    Console.ReadKey();
            //}
        }
    }
}
