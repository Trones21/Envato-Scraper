using IronWebScraper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyWebScraper
{
    [Table("StatsViaPP", Schema = "Product")]
    public class ProductStatsViaPP
    {
        [Key, Column(Order = 0, TypeName = "Date")]
        public DateTime Date_ofScrape { get; set; }
        [Key, Column(Order = 1)]
        public string ProductID { get; set; }

        [Column(TypeName = "Date")]
        public DateTime Date_LastUpdatedbyAuthor { get; set; }
        public string Hierarchy { get; set; }
        public int SalesCount { get; set; }
        /// <summary>
        /// Ratings are the same as reviews. It's just different terminology used on different pages.
        /// </summary>
        public int RatingsCount { get; set; }
        public int CommentsCount { get; set; }
        public decimal AvgRating { get; set; }
        public decimal percent5star { get; set; }
        public decimal percent4star { get; set; }
        public decimal percent3star { get; set; }
        public decimal percent2star { get; set; }
        public decimal percent1star { get; set; }
        public int TagsCount { get; set; }
        /// <summary>
        /// Use these fields to update the Product Table, 
        /// </summary>
        public bool hasComments;
        public bool hasReviews;

        public void SetValues(Response response)
        {
            var metaAttr = response.QuerySelector(".meta-attributes__table");
            if (metaAttr.QuerySelectorAll(".meta-attributes__attr-name")[0].InnerText.Trim() == "Last Update")
            {
                this.Date_LastUpdatedbyAuthor = Convert.ToDateTime(metaAttr.QuerySelectorAll(".meta-attributes__attr-detail")[0].InnerText);
            }

            foreach (var item in response.QuerySelectorAll(".sidebar-stats__item"))
            {
                switch (item.QuerySelector(".sidebar-stats__label").InnerText.Trim())
                {
                    case "Sales":
                        this.SalesCount = Convert.ToInt32(item.QuerySelector(".sidebar-stats__number").InnerText.Replace(",", "").Trim());
                        break;
                    case "Comments":
                        this.CommentsCount = Convert.ToInt32(item.QuerySelector(".sidebar-stats__number").InnerText.Replace(",", "").Trim());
                        break;
                }
            }

            if (response.CssExists(".rating-detailed"))
            {
                var ratingsNode = response.QuerySelector(".rating-detailed");
                var parse = ratingsNode.QuerySelector(".rating-detailed__average").InnerText.Trim();


                this.AvgRating = Convert.ToDecimal(parse.Substring(0, 4));

                var temp = parse.Split(new string[] { "on" }, StringSplitOptions.None)[1];
                temp = Regex.Match(temp, "[0-9]+").ToString();
                this.RatingsCount = Convert.ToInt32(temp);

                var starNodes = ratingsNode.QuerySelector("ul[data-view]").QuerySelectorAll("li");
                foreach (var starNode in starNodes)
                {
                    switch (starNode.QuerySelector(".rating-breakdown__key").InnerText.Trim())
                    {

                        case "5 Star":
                            this.percent5star = Convert.ToDecimal(starNode.QuerySelector(".rating-breakdown__count").InnerText.Replace("%", "").Trim()) / 100;
                            break;
                        case "4 Star":
                            this.percent4star = Convert.ToDecimal(starNode.QuerySelector(".rating-breakdown__count").InnerText.Replace("%", "").Trim()) / 100;
                            break;
                        case "3 Star":
                            this.percent3star = Convert.ToDecimal(starNode.QuerySelector(".rating-breakdown__count").InnerText.Replace("%", "").Trim()) / 100;
                            break;
                        case "2 Star":
                            this.percent2star = Convert.ToDecimal(starNode.QuerySelector(".rating-breakdown__count").InnerText.Replace("%", "").Trim()) / 100;
                            break;
                        case "1 Star":
                            this.percent1star = Convert.ToDecimal(starNode.QuerySelector(".rating-breakdown__count").InnerText.Replace("%", "").Trim()) / 100;
                            break;
                    }

                    

                }
                this.TagsCount = metaAttr.QuerySelector(".meta-attributes__attr-tags").GetElementsByTagName("a").Count();
            }
        }

        public bool WritetoDB()
        {
            try
            {
                using (var db = new WebScrapeDbContext())
                {
                    db.Product_StatsViaProfilePage.Add(this);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Program.SummaryEmailer.AddText("\n Failed to write to DB, ProductStats: " + this.ProductID);
                return false;
            }
        }
    }
}
