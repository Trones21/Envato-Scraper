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
    ///  Found in div class="meta-attributes"
    ///  **** Ignore Last Update, Created, Tags -- These are stored separately
    /// </summary>
    [Table("Attributes", Schema = "Product")]
    public class ProductAttribute
    {

        [Key, Column(Order = 0)]
        public string ProductID { get; set; }
        [Key, Column(Order = 1)]
        public string AttributeName { get; set; }
        /// <summary>
        /// If multiple, then it concatenates -- Keep it simple for now
        /// </summary>
        [Key, Column(Order = 2)]
        public string AttributeValue { get; set; }
        public string ChangeSetID { get; set; }

        //Scrape <div class="meta-attributes">
        // Ignore Last Update, Created, Tags -- These are stored separately

        //These are operations will be done in the database, but here is the logic
        //if ProductAttrNameID does not exist, then add it
        //ChangeSetID = 1
        //elseif ProductAttrNameValueID does not exist then add it
        //ChangeSetID =  MAX(ChangeSetID) + 1  where DB.ProductAttrNameID = Scrape.ProductAttrNameID
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node">tr Node in tbody</param>
        public void SetValues(HtmlNode node, string productID)
        {
            this.AttributeName = node.QuerySelector(".meta-attributes__attr-name").InnerText;
            this.AttributeValue = node.QuerySelector(".meta-attributes__attr-detail").TextContent.Trim();
            this.ProductID = productID;
        }


    }
}
