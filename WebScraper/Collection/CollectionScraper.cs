using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;

namespace MyWebScraper
{
    public class CollectionScraper : WebScraper
    {
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
