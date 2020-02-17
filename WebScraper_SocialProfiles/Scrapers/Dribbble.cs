using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;

namespace WebScraper_SocialProfiles.Scrapers
{
    class Dribbble
    {
        string isProMember;
        string ShotsCount;
        string FollowersCount;
        string GoodsForSaleCount;
        List<string> Links;
        List<Shot> shots;
        List<Tag> Tags;

        //One Time
        string ProfileName;
        string Location;
        
    }

    internal class Shot
    {
        string Name;
        string UploadDate;
        string Description;

        string ViewCount;
        string CommentCount;
        string LikeCount;
     
    }

    internal class Tag
    {
        //PK_BizLogic: These are decoupled from "Shots", must login to see detailed shot info 
        string TagName;
        int UsageCount;
    }


    class DribbbleScraper : WebScraper
    {
        public override void Init()
        {
            this.Identities = MyWebScraper.Program.HttpIdentities();
            this.LoggingLevel = WebScraper.LogLevel.Critical;

            this.Request("https://dribbble.com/wahoooos", Parse);
        }

        public override void Parse(Response response)
        {
        }
    }
}



