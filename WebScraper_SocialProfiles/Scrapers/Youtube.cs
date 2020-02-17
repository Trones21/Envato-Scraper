using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;

namespace WebScraper_SocialProfiles.Scrapers
{
    class Youtube
    {
        string subscriberCount;
        List<video> Videos;
    }

    class video
    {
        string Name;
        string viewCount;
        string Length;
        string TimeOnYoutube;
    }

    class YoutubeScraper : WebScraper
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
