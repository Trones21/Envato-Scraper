using IronWebScraper;
using System;

namespace MyWebScraper
{
    public class AuthorViaListPageCountScraper : WebScraper
    {
        public int totalPages = new int();

        public override void Init()
        {
            this.Identities = Program.HttpIdentities();
            this.LoggingLevel = WebScraper.LogLevel.Critical;           
            this.Request("https://videohive.net/authors/top?site=all&level=all", Parse);

        }
        public override void Parse(Response response)
        {
            HtmlNode[] paginationList = response.Css(".pagination__list");
            HtmlNode[] childnodes = paginationList[0].ChildNodes;
            string totalpages = childnodes[16].TextContent.Trim(); //Text and link nodes exist, so number is double
            totalPages = Convert.ToInt32(totalpages);

        }
    }
    public class CollectionsPageCountScraper : WebScraper
        {
            public int totalPages = new int();

            public override void Init()
            {
                this.Identities = Program.HttpIdentities();
                this.LoggingLevel = WebScraper.LogLevel.Critical;

            }
            public override void Parse(Response response)
            {

            }

        }
    }
