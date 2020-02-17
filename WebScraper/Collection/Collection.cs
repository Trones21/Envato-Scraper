using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;
using System.ComponentModel.DataAnnotations;

namespace MyWebScraper
{
    public class Collection
    {
        //PK_ToDo -Need to change fields to Int and always Convert.ToInt 
        //On List Page
        
        [Key]
        public string Link { get; set; }
        public string ImageLink { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }

        public int itemsCount { get; set; }
        public int ratingsCount { get; set; }

        //On Collection Page
        public string FullDescription;
        public List<ProductBasic> ProductBasic;
        public string CanRateEntireCollection;
        
        /// <summary>
        /// Send the inidividual collection-summary element as a parameter
        /// </summary>
        /// <param name="collectionSummary"></param>
        public void SetValuesfromList(HtmlNode[] collectionSummary)
        {
            var imglink = collectionSummary[1].GetElementsByTagName("img");
                this.ImageLink = imglink[1].GetAttribute("src");

            var info = collectionSummary.CSS(".collection-summary__info");          
                this.Link = info[1].GetElementsByTagName("h3")[0].GetElementsByTagName("a")[0].GetAttribute("href");
                this.Name = info[1].GetElementsByTagName("h3")[0].GetElementsByTagName("a")[0].InnerTextClean;       
            var ownerRaw = info[3].GetElementsByTagName("a");
                this.Owner = ownerRaw[0].InnerTextClean;

            var meta = collectionSummary.CSS(".collection-summary__meta");           
                this.itemsCount = Convert.ToInt32(meta[1].InnerTextClean.Split(' ')[0]);
                this.ratingsCount = Convert.ToInt32(meta[5].Css(".rating-basic__count")[0].InnerTextClean.Split(' ')[0]);
               
        }
        public void SetValuesFromPage()
        {

        }




    }

    public class ProductBasic
    {
        public string Name;
        public string Author;
        public string ListedPrice;
        //Can get rest of info by merging with Product full 
    }


}
