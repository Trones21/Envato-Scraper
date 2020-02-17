using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronWebScraper;

namespace MyWebScraper
{
    [Table("Reviews", Schema = "Product")]
    public class Review
    {

        [Key]
        public string ReviewID { get; set; }
        public string ProductID { get; set; }
        [Column(TypeName = "Date")]
        public DateTime Date_ofScrape { get; set; }
        public string TimeSinceCreation { get; set; }
        public string Reviewer { get; set; }
        public string Reason { get; set; }
        public string Text { get; set; }
        public int Stars { get; set; } //need to check which list item contains -color-grey-medium to get the star rating.
        public bool hasAuthorResponded { get; set; } //One value proposition is that I could show them which low star reviews they have not responded to.


        public void SetValues(HtmlNode htmlNode, string ProductID)
        {

            this.ProductID = ProductID;
            this.ReviewID = htmlNode.GetAttribute("id").Replace("review_", "");
            this.Date_ofScrape = DateTime.Now;
            this.TimeSinceCreation = htmlNode.QuerySelector(".review-header__date > a").InnerText;
            
            this.Reviewer = htmlNode.QuerySelector(".review-header__reviewer > p > a").InnerText;
            this.Reason = htmlNode.QuerySelector(".h-m0").InnerText.Trim().Substring(4);

            if (htmlNode.CssExists(".e-box.h-p2 > p")){
                this.Text = htmlNode.QuerySelector(".e-box.h-p2 > p").InnerText;
                }else{
                this.Text = "None";}

            var stars = htmlNode.QuerySelector(".rating-basic__star-rating");
            //might have to filter this down to where NodeName = I or NodeType = 1 check IWS docs
            if (stars.QuerySelectorAll(".e-icon.-icon-star.-color-grey-medium") is null == false) //not going to work
            {
                this.Stars = 5 - stars.QuerySelectorAll(".e-icon.-icon-star.-color-grey-medium").Count();
            }
            else
            {
                this.Stars = 5;
            }

            this.hasAuthorResponded = htmlNode.CssExists(".media__body");

            Debug.WriteLine("Review scraped");

        }
    }
}
