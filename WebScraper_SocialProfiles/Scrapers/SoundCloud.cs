using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;

namespace WebScraper_SocialProfiles.Scrapers
{
    class SoundCloud
    {
        int FollowersCount;
        int FollowingCount;
    }

    internal class Track
    {
        int Likes;
        int Reposts;
        int Plays;
        int Comments;
    }

    class SoundCloudScraper : WebScraper
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
