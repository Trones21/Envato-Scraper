using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;

namespace WebScraper_SocialProfiles.Scrapers
{
    class Twitter
    {
     string tweetsCount;
     string followersCount;
     string LatestTweetDate;
     string Link;

     //One Time
     string Location;
     string JoinDate;
    }

    class TwitterScraper : WebScraper
    {
        public override void Init()
        {
            this.Identities = MyWebScraper.Program.HttpIdentities();
            this.LoggingLevel = WebScraper.LogLevel.Critical;

            this.Request("https://www.youtube.com/user/soundrollmusic", Parse);
        }

        public override void Parse(Response response)
        {
        }
    }
}
