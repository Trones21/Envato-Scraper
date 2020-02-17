using IronWebScraper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebScraper
{
    [Table("Tags", Schema = "Product")]
    public class Tag
    {
        [Key, Column(Order = 0)]
        public string ProductID { get; set; }
        [Key, Column(Order = 1)]
        public string TagName { get; set; }
        [Column(TypeName = "Date")]
        public DateTime date_ofScrape { get; set; }

        public void SetValues(HtmlNode node, string ProductID)
        {
            this.ProductID = ProductID;
            TagName = node.GetAttribute("title");
            date_ofScrape = DateTime.Now;
        }
    }
}
