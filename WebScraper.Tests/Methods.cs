using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;
using MyWebScraper;

namespace WebScraper.Tests
{
    public static class Methods
    {

        public static Response MakeRequest(string URL)
        {
            var scraper = new Scraper();
            scraper.Request(URL, scraper.Parse);
            scraper.Start();

            return scraper.response_t;
        }
    }
}
