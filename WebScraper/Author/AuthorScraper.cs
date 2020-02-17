using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;

namespace MyWebScraper
{
    public class AuthorScraper : WebScraper
    {

        public List<Author> Authors = new List<Author>();

        public override void Init()
        {
            var httpIdentities = new List<HttpIdentity>();

            HttpIdentity httpIdentity = new HttpIdentity();
            httpIdentity.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";
            httpIdentities.Add(httpIdentity);

            this.Identities = httpIdentities;
            this.LoggingLevel = WebScraper.LogLevel.All;



            this.Request("https://videohive.net/user/gorodenkoffs", Parse);

        }

        public override void Parse(Response response)
        {
            /*
            AuthorFull author = new AuthorFull();

            author.memberSince = response.QuerySelector("#content > div:nth-child(1) > section > div.grid-container > div.user-info-header.h-mb0 > div.user-info-header__user-details > div > p").InnerTextClean;
            author.Name = response.QuerySelector("#content > div:nth-child(1) > section > div.grid-container > div.user-info-header.h-mb0 > div.user-info-header__user-details > div > a > h2").InnerTextClean;
            author.Rating = response.QuerySelector("#content > div:nth-child(1) > section > div.grid-container > div.user-info-header.h-mb0 > div.user-info-header__user-stats > div.user-info-header__stats-article.h-mx2 > div > div.user-info-header__stats-content.-extra-padding > div > span").InnerTextClean;
            author.RatingsCount = response.QuerySelector("#content > div:nth-child(1) > section > div.grid-container > div.user-info-header.h-mb0 > div.user-info-header__user-stats > div.user-info-header__stats-article.h-mx2 > div > div.user-info-header__stats-content.-extra-padding > span").InnerTextClean;
            author.SalesCount = response.QuerySelector("#content > div:nth-child(1) > section > div > div.user-info-header.h-mb0 > div.user-info-header__user-stats > div.user-info-header__stats-article.h-mr0.h-ml2 > div.user-info-header__stats-content > strong").InnerTextClean;

            //Use XPath when the classes might be different
            HtmlNode[] followersNode = response.XPath("//*[@id=\"content\"]/div[1]/section/div/div[2]/div/ul/li[3]/a/span");
            HtmlNode[] followingNode = response.XPath("//*[@id=\"content\"]/div[1]/section/div/div[2]/div/ul/li[4]/a/span");
            author.followersCount = followersNode[0].TextContent;
            author.followingCount = followingNode[0].TextContent;

            HtmlNode[] weblinktoProfilePhoto = response.XPath("//*[@id=\"content\"]/div[1]/section/div/div[1]/div[1]/img");
            var nodeattr = weblinktoProfilePhoto[0].ChildNodes[0].Attributes;
            string value = "PK_NoImage";
            if (nodeattr.TryGetValue("src", out value))
            {
                author.logoweblink = value;
            }

            Authors.Add(author);
            */

        }
    }
}