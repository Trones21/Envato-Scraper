using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;

namespace WebScraper_SocialProfiles.Scrapers
{
    class Behance
    {
        int ProjectViews;
        int AppreciationsCount;
        int FollowersCount;
        int FollowingCount;
        string profileTitle;
        List<Project> Projects;
        //One Time
        string profileName;
        
    }

    internal class Project
    {
        string Title;
        string HasBeenFeatured;
        string FeaturedDate;
        int Likes;
        int Views;
    }

    class BehanceScraper : WebScraper
    {
        public override void Init()
        {
            this.Identities = MyWebScraper.Program.HttpIdentities();
            this.LoggingLevel = WebScraper.LogLevel.Critical;

            this.Request("https://behance.net/kontramax", Parse);
        }

        public override void Parse(Response response)
        {
        }
    }
}

