using IronWebScraper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebScraper
{
    [Table("Comments", Schema = "Product")]
    public class Comment
    {
        [Key]
        public string CommentID { get; set; }
        public string ProductID { get; set; }
        [Column(TypeName = "Date")]
        public DateTime Date_ofScrape { get; set; }
        public string Commenter { get; set; }
        public string TimeSinceCreation { get; set; }
        public string Text { get; set; }
        public bool hasCommenterPurchased { get; set; }
        public bool hasAuthorResponded { get; set; }

        /// <summary>
        /// Class is js-discussion comment__container-redesign
        /// </summary>
        /// <param name="htmlNode">Class is js-discussion comment__container-redesign</param>
        /// <param name="URL"></param>
        public void SetValues(HtmlNode htmlNode, string ProductID)
        {
            this.ProductID = ProductID;
            this.CommentID = htmlNode.QuerySelector(".comment__info").GetAttribute("id").Replace("comment_", "");
           
            this.Commenter = htmlNode.QuerySelector(".media__body > p > a").InnerText;
            this.Date_ofScrape = DateTime.Now;
            this.TimeSinceCreation = htmlNode.QuerySelector(".comment__date").InnerText;
            this.Text = htmlNode.QuerySelector(".comment__body").InnerTextClean;

            this.hasCommenterPurchased = htmlNode.CssExists(".-color-grey-dark");
            this.hasAuthorResponded = htmlNode.CssExists(".-color-grey");

            Debug.WriteLine("Comment Scraped");
        }
    }
}
