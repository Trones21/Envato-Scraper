using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;

namespace WebScraper_SocialProfiles.Scrapers
{
    class Flickr
    {
        string followersCount;
        string website;
    }

    class FlickrScraper : WebScraper
    {
        public override void Init()
        {
            this.Identities = MyWebScraper.Program.HttpIdentities();
            this.LoggingLevel = WebScraper.LogLevel.Critical;

            this.Request("", Parse);
        }

        public override void Parse(Response response)
        {
        }
    }
}
