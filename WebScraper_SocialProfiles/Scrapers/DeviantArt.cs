using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;

namespace WebScraper_SocialProfiles.Scrapers
{
    class DeviantArt
    {

        //One Time
        //https://dribbble.com/wahoooos
    }

    class DeviantArtScraper : WebScraper
    {
        public override void Init()
        {
            this.Identities = MyWebScraper.Program.HttpIdentities();
            this.LoggingLevel = WebScraper.LogLevel.Critical;

            this.Request("http://Party-Flyer.deviantart.com/", Parse);
        }

        public override void Parse(Response response)
        {
        }
    }
}


