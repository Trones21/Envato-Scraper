using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;
using MyWebScraper;

namespace WebScraper_SocialProfiles.Scrapers
{

    class Facebook
    {
        string LikesCount;
        string followersCount;

        string FoundedDate;
        string emailAddress;
        string website;
    }

    class FacebookScraper : WebScraper
    {
        public override void Init()
        {
            this.Identities = MyWebScraper.Program.HttpIdentities();
            this.LoggingLevel = WebScraper.LogLevel.Critical;

            this.Request("https://www.facebook.com/MuffinGroup", Parse);
        }

        public override void Parse(Response response)
        {
            //Having trouble Using Css className selectors
            //if (response.CssExists()

        }
    }
}
