using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MyWebScraper;
using IronWebScraper;
using WebScraper_SocialProfiles.Scrapers;

namespace WebScraper_SocialProfiles
{
    class Program
    {
        static void Main(string[] args)
        {
            List<HttpIdentity> httpIdentities = MyWebScraper.Program.HttpIdentities();
            FacebookScraper facebookScraper = new FacebookScraper();

            List<Facebook> FacebookProfiles = new List<Facebook>();
            facebookScraper.StartAsync();
            
        }
    }
}
