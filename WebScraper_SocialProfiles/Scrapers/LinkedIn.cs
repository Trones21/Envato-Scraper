using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;

namespace WebScraper_SocialProfiles.Scrapers
{
    class LinkedIn
    {
        //PK_Scraper: Looks like profiles might only be visible after you login
    }
    class LinkedInScraper : WebScraper
    {
        public override void Init()
        {
            this.Identities = MyWebScraper.Program.HttpIdentities();
            this.LoggingLevel = WebScraper.LogLevel.Critical;

            this.Request("https://soundcloud.com/Afterdarkness75", Parse);
        }

        public override void Parse(Response response)
        {
        }
    }
}
