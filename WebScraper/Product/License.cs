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
    /// <summary>
    /// ChangesetID write to DB logic:
    ///if the Key(ProductID_LicenseName_Price) already exists in the DB, then don't write, else write
    /// </summary>
    [Table("Licenses", Schema = "Product")]
    public class License
    {
        [Key, Column(Order = 0)]
        public string ProductID { get; set; }
        [Key, Column(Order = 1)]
        public string LicenseName { get; set; }
        //Keep this as a string just in case the proxies cause it to get different Currencies     
        [Key, Column(Order = 2)]
        public string Price { get; set; }

        [Column(TypeName = "Date")]
        public DateTime Date_ofScrape { get; set; }
        /// <summary>
        /// This should be calculated in the database, no operations done here
        /// </summary>
        public string ChangesetID { get; set; }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="node">response.QuerySelectorAll(".purchase-form__license")[index]</param>
        /// <param name="ProductID"></param>
        public void SetValues(HtmlNode node)
        {           
            Date_ofScrape = DateTime.Now;
            LicenseName = node.GetAttribute("data-license");
            Price = node.QuerySelector(".js-purchase-license-prices").GetAttribute("data-price-prepaid");
        }
        public void SetValues(HtmlNode node, string ProductID)
        {
            this.ProductID = ProductID;
            Date_ofScrape = DateTime.Now;
            LicenseName = node.GetAttribute("data-license");
            Price = node.QuerySelector(".js-purchase-license-prices").GetAttribute("data-price-prepaid");
        }

    }
}
